using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Filters;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using System.Linq;

namespace MoviesApp.Controllers
{
	public class ActorsController : Controller
	{
		private readonly MoviesContext _context;
		private readonly ILogger<ActorsController> _logger;

		public ActorsController(MoviesContext context, ILogger<ActorsController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View(_context.Actors
				.Include(actor => actor.MoviesActors)
				.Select(actor => new ActorViewModel
				{
					Id = actor.Id,
					FirstName = actor.FirstName,
					LastName = actor.LastName,
					Age = actor.Age,
					Birthday = actor.Birthday,
					Town = actor.Town,
					MoviesActors = actor.MoviesActors
				}).ToList());
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[CheckAgeActorsFilter]
		[ValidateAntiForgeryToken]
		public IActionResult Create([Bind("FirstName,LastName,Age,Birthday,Town")] CreateActorViewModel inputModel)
		{
			if (ModelState.IsValid)
			{
				_context.Add(new Actor
				{
					FirstName = inputModel.FirstName,
					LastName = inputModel.LastName,
					Age = inputModel.Age,
					Birthday = inputModel.Birthday,
					Town = inputModel.Town
				});
				_context.SaveChanges();

				return RedirectToAction(nameof(Index));
			}
			return View(inputModel);
		}

		[HttpGet]
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var viewModel = _context.Actors
				.Where(actor => actor.Id == id)
				.Include(actor => actor.MoviesActors)
					.ThenInclude(m => m.Movie)
				.Select(actor => new ActorViewModel
				{
					Id = actor.Id,
					FirstName = actor.FirstName,
					LastName = actor.LastName,
					Age = actor.Age,
					Birthday = actor.Birthday,
					Town = actor.Town,
					MoviesActors = actor.MoviesActors
				}).FirstOrDefault();


			if (viewModel == null)
			{
				return NotFound();
			}

			return View(viewModel);
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var editModel = _context.Actors
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Movie)
				.Where(m => m.Id == id)
				.Select(m => new EditActorViewModel
				{
					FirstName = m.FirstName,
					LastName = m.LastName,
					Age = m.Age,
					Birthday = m.Birthday,
					Town = m.Town,
					MoviesActors = m.MoviesActors

				}).FirstOrDefault();

			if (editModel == null)
			{
				return NotFound();
			}

			return View(editModel);
		}

		[HttpPost]
		[CheckAgeActorsFilter]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, [Bind("FirstName,LastName,Age,Birthday,Town")] EditActorViewModel editModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var actor = new Actor
					{
						Id = id,
						FirstName = editModel.FirstName,
						LastName = editModel.LastName,
						Age = editModel.Age,
						Birthday = editModel.Birthday,
						Town = editModel.Town
					};

					_context.Update(actor);
					_context.SaveChanges();
				}
				catch (DbUpdateException)
				{
					if (!ActorExists(id))
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
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var deleteModel = _context.Actors.Where(m => m.Id == id).Select(m => new DeleteActorViewModel
			{
				FirstName = m.FirstName,
				LastName = m.LastName,
				Age = m.Age,
				Birthday = m.Birthday,
				Town = m.Town
			}).FirstOrDefault();

			if (deleteModel == null)
			{
				return NotFound();
			}

			return View(deleteModel);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			var actor = _context.Actors.Find(id);
			_context.Actors.Remove(actor);
			_context.SaveChanges();
			_logger.LogError($"Movie with id {actor.Id} has been deleted!");
			return RedirectToAction(nameof(Index));
		}

		[HttpPost, ActionName("Details")]
		[ValidateAntiForgeryToken]
		public IActionResult Detach(int id, int movieId)
		{
			var movie = _context.MoviesActors
				.Where(m => m.ActorId == id)
				.Where(a => a.MovieId == movieId)
				.FirstOrDefault();

			_context.MoviesActors.Remove(movie);
			_context.SaveChanges();

			return RedirectToAction(nameof(Details));
		}

		private bool ActorExists(int id)
		{
			return _context.Actors.Any(e => e.Id == id);
		}
	}
}
