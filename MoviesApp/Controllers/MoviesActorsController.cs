using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using System;
using System.Linq;

namespace MoviesApp.Controllers
{
	public class MoviesActorsController : Controller
	{
		private readonly MoviesContext _context;
		private readonly ILogger<MoviesActorsController> _logger;

		public MoviesActorsController(MoviesContext context, ILogger<MoviesActorsController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult AddActor(int? id)
		{
			if (id == null)
				return NotFound();

			var actors = _context.Actors.ToList();

			var movies = _context.Movies
				.Where(m => m.Id == id)
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Actor)
				.Select(m => new MovieViewModel
				{
					Actors = m.MoviesActors

				}).FirstOrDefault();

			if (movies == null)
				return NotFound();

			var listed = actors.Select(a => a.Id).ToList().Except(movies.Actors.Select(a => a.ActorId).ToList());

			var actorList = _context.Actors
				.Where(a => listed.Contains(a.Id))
				.ToList();

			if (actorList == null)
				return NotFound();

			return View(actorList);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddActor(int id, int actorId)
		{
			try
			{
				var movieActors = new MoviesActors
				{
					ActorId = actorId,
					MovieId = id
				};

				_context.Update(movieActors);
				_context.SaveChanges();
			}
			catch (DbUpdateException)
			{
				throw;
			}

			return RedirectToAction(nameof(AddActor));
		}

		[HttpGet]
		public IActionResult AddMovie(int? id)
		{
			if (id == null)
				return NotFound();

			var actors = _context.Movies.ToList();

			var movies = _context.Actors
				.Where(m => m.Id == id)
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Movie)
				.Select(m => new ActorViewModel
				{
					MoviesActors = m.MoviesActors
				}).FirstOrDefault();

			if (movies == null)
				return NotFound();

			var listed = actors.Select(a => a.Id).ToList().Except(movies.MoviesActors.Select(a => a.MovieId).ToList());

			var movieList = _context.Movies
				.Where(a => listed.Contains(a.Id))
				.ToList();

			if (movieList == null)
				return NotFound();

			return View(movieList);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddMovie(int id, int movieId)
		{
			try
			{
				var movieActors = new MoviesActors
				{
					ActorId = id,
					MovieId = movieId
				};

				_context.Update(movieActors);
				_context.SaveChanges();
			}
			catch (DbUpdateException)
			{
				throw;
			}

			return RedirectToAction(nameof(AddMovie));
		}
	}
}
