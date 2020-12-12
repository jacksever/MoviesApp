using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
	public class MoviesController : Controller
	{
		private readonly MoviesContext _context;
		private readonly ILogger<MoviesController> _logger;

		public MoviesController(MoviesContext context, ILogger<MoviesController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpGet]
		[Authorize]
		public IActionResult Index()
		{
			return View(_context.Movies
				.Include(movie => movie.MoviesActors)
				.Select(m => new MovieViewModel
				{
					Id = m.Id,
					Genre = m.Genre,
					Price = m.Price,
					Title = m.Title,
					ReleaseDate = m.ReleaseDate,
					Actors = m.MoviesActors
				}).ToList());
		}

		[HttpGet]
		[Authorize]
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var viewModel = _context.Movies
				.Where(m => m.Id == id)
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Actor)
				.Select(m => new MovieViewModel
				{
					Id = m.Id,
					Genre = m.Genre,
					Price = m.Price,
					Title = m.Title,
					ReleaseDate = m.ReleaseDate,
					Actors = m.MoviesActors
				}).FirstOrDefault();


			if (viewModel == null)
			{
				return NotFound();
			}

			return View(viewModel);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public IActionResult Create([Bind("Title,ReleaseDate,Genre,Price")] InputMovieViewModel inputModel)
		{
			if (ModelState.IsValid)
			{
				_context.Add(new Movie
				{
					Genre = inputModel.Genre,
					Price = inputModel.Price,
					Title = inputModel.Title,
					ReleaseDate = inputModel.ReleaseDate
				});
				_context.SaveChanges();

				return RedirectToAction(nameof(Index));
			}
			return View(inputModel);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var editModel = _context.Movies.Where(m => m.Id == id).Select(m => new EditMovieViewModel
			{
				Genre = m.Genre,
				Price = m.Price,
				Title = m.Title,
				ReleaseDate = m.ReleaseDate
			}).FirstOrDefault();

			if (editModel == null)
			{
				return NotFound();
			}

			return View(editModel);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, [Bind("Title,ReleaseDate,Genre,Price")] EditMovieViewModel editModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var movie = new Movie
					{
						Id = id,
						Genre = editModel.Genre,
						Price = editModel.Price,
						Title = editModel.Title,
						ReleaseDate = editModel.ReleaseDate
					};

					_context.Update(movie);
					_context.SaveChanges();
				}
				catch (DbUpdateException)
				{
					if (!MovieExists(id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(editModel);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var deleteModel = _context.Movies.Where(m => m.Id == id).Select(m => new DeleteMovieViewModel
			{
				Genre = m.Genre,
				Price = m.Price,
				Title = m.Title,
				ReleaseDate = m.ReleaseDate
			}).FirstOrDefault();

			if (deleteModel == null)
			{
				return NotFound();
			}

			return View(deleteModel);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteConfirmed(int id)
		{
			var movie = _context.Movies.Find(id);
			_context.Movies.Remove(movie);
			_context.SaveChanges();
			_logger.LogError($"Movie with id {movie.Id} has been deleted!");
			return RedirectToAction(nameof(Index));
		}

		[HttpPost, ActionName("Details")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public IActionResult Detach(int id, int actorId)
		{
			var movie = _context.MoviesActors
				.Where(m => m.ActorId == actorId)
				.Where(a => a.MovieId == id)
				.FirstOrDefault();

			_context.MoviesActors.Remove(movie);
			_context.SaveChanges();

			return RedirectToAction(nameof(Details));
		}

		private bool MovieExists(int id)
		{
			return _context.Movies.Any(e => e.Id == id);
		}
	}
}