using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApp.Services
{
	public class ActorWithMovieService : IActorWithMovieService
	{
		private readonly MoviesContext _context;
		private readonly IMapper _mapper;

		public ActorWithMovieService(MoviesContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public bool Detach(int actorId, int movieId)
		{
			var item = _context.MoviesActors
				.Where(m => m.ActorId == actorId)
				.Where(m => m.MovieId == movieId)
				.FirstOrDefault();

			if (item == null)
				return false;

			_context.MoviesActors.Remove(item);
			_context.SaveChanges();

			return true;
		}

		public IEnumerable<ActorDto> AddActor(int id)
		{
			var actors = _context.Actors.ToList();

			if (actors == null)
				return null;

			var movies = _context.Movies
				.Where(m => m.Id == id)
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Actor)
				.Select(m => new MovieWithActorDto
				{
					MoviesActors = m.MoviesActors

				}).FirstOrDefault();

			if (movies == null)
				return null;

			var listed = actors.Select(a => a.Id).ToList().Except(movies.MoviesActors.Select(a => a.ActorId).ToList());

			var actorList = _mapper.Map<IEnumerable<Actor>, IEnumerable<ActorDto>>(_context.Actors.Where(a => listed.Contains(a.Id)).ToList());

			return actorList;
		}

		public IEnumerable<MovieDto> AddMovie(int id)
		{

			var movies = _context.Movies.ToList();

			if (movies == null)
				return null;

			var actors = _context.Actors
				.Where(m => m.Id == id)
				.Include(m => m.MoviesActors)
					.ThenInclude(mm => mm.Movie)
				.Select(m => new MovieWithActorDto
				{
					MoviesActors = m.MoviesActors
				}).FirstOrDefault();

			if (actors == null)
				return null;

			var listed = movies.Select(a => a.Id).ToList().Except(actors.MoviesActors.Select(a => a.MovieId).ToList());

			var movieList = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDto>>(_context.Movies.Where(m => listed.Contains(m.Id))).ToList();

			return movieList;
		}

		public bool Attach(int actorId, int movieId)
		{
			try
			{
				var item = new MoviesActors
				{
					ActorId = actorId,
					MovieId = movieId
				};

				_context.Update(item);
				_context.SaveChanges();
			}
			catch (DbUpdateException)
			{
				return false;
			}

			return true;
		}
	}
}
