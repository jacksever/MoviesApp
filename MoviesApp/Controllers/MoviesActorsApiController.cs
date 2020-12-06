using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApp.Controllers
{
	[ApiController]
	[Route("api/moviesActors")]
	public class MoviesActorsApiController : ControllerBase
	{
		private readonly MoviesContext _context;
		private readonly IMapper _mapper;

		public MoviesActorsApiController(MoviesContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet("addActor/{id}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ApiUpdateActorViewModel>))]
		[ProducesResponseType(404)]
		public ActionResult<IEnumerable<ApiUpdateActorViewModel>> AddActor(int? id)
		{
			if (id == null)
				return BadRequest();

			var actors = _mapper.Map<IEnumerable<Actor>, IEnumerable<ApiActorViewModel>>(_context.Actors.ToList());

			var movies = _mapper.Map<Movie, ApiMovieWithActorViewModel>(_context.Movies
				.Where(m => m.Id == id)
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Actor)
				.FirstOrDefault());

			if (movies == null)
				return NotFound();

			var listed = actors.Select(a => a.Id).ToList().Except(movies.Actors.Select(a => a.ActorId).ToList());

			var actorList = _mapper.Map<IEnumerable<Actor>, IEnumerable<ApiUpdateActorViewModel>>(_context.Actors
				.Where(a => listed.Contains(a.Id))
				.ToList());

			if (actorList == null)
				return NotFound();

			return Ok(actorList);

		}

		[HttpPost("addActor/{movieId}/{actorId}")]
		public IActionResult AddActor(int movieId, int actorId)
		{
			var actorMovies = _context.MoviesActors
				.Where(m => m.ActorId == actorId && m.MovieId == movieId)
				.FirstOrDefault();

			if (actorMovies == null)
			{
				try
				{
					var movieActors = new MoviesActors
					{
						ActorId = actorId,
						MovieId = movieId
					};

					_context.Update(movieActors);
					_context.SaveChanges();
				}
				catch (DbUpdateException)
				{
					throw;
				}
			}
			else
				return BadRequest("The selected movie already has this actor!");

			return Ok("The actor was successfully added to this movie :)");
		}

		[HttpGet("addMovie/{id}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ApiMovieByIdViewModel>))]
		[ProducesResponseType(404)]
		public ActionResult<ApiMovieByIdViewModel> AddMovie(int? id)
		{
			if (id == null)
				return NotFound();

			var movies = _mapper.Map<IEnumerable<Movie>, IEnumerable<ApiMovieByIdViewModel>>(_context.Movies.ToList());

			var actors = _mapper.Map<Actor, ApiActorWithMovieViewModel>(_context.Actors
				.Where(a => a.Id == id)
				.Include(a => a.MoviesActors)
					.ThenInclude(aa => aa.Movie)
				.FirstOrDefault());

			if (actors == null)
				return NotFound();

			var listed = movies.Select(a => a.Id).ToList().Except(actors.Movies.Select(a => a.MovieId).ToList());

			var movieList = _mapper.Map<IEnumerable<Movie>, IEnumerable<ApiMovieByIdViewModel>>(_context.Movies
				.Where(a => listed.Contains(a.Id))
				.ToList());

			if (movieList == null)
				return NotFound();

			return Ok(movieList);
		}

		[HttpPost("addMovie/{actorId}/{movieId}")]
		public IActionResult AddMovie(int actorId, int movieId)
		{
			var actorMovies = _context.MoviesActors
				.Where(m => m.ActorId == actorId && m.MovieId == movieId)
				.FirstOrDefault();

			if (actorMovies == null)
			{
				try
				{
					var movieActors = new MoviesActors
					{
						ActorId = actorId,
						MovieId = movieId
					};

					_context.Update(movieActors);
					_context.SaveChanges();
				}
				catch (DbUpdateException)
				{
					throw;
				}
			}
			else
				return BadRequest("The selected actor already has this movie!");

			return Ok("This movie is successfully attached to this actor");
		}


		[HttpDelete("detach/{actorId}/{movieId}")]
		public IActionResult Detach(int actorId, int movieId)
		{
			var content = _context.MoviesActors
				.Where(m => m.ActorId == actorId)
				.Where(a => a.MovieId == movieId)
				.FirstOrDefault();

			if (content == null)
				return BadRequest("The actor or the film is not attached or not found!");

			_context.MoviesActors.Remove(content);
			_context.SaveChanges();

			return Ok("Actor and movie successfully detached from each other :)");
		}
	}
}
