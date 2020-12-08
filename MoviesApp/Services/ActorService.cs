using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Dto;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApp.Services
{
	public class ActorService : IActorService
	{
		private readonly MoviesContext _context;
		private readonly IMapper _mapper;

		public ActorService(MoviesContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IEnumerable<ActorDto> GetAllActor()
		{
			return _mapper.Map<IEnumerable<Actor>, IEnumerable<ActorDto>>(_context.Actors
				.Include(a => a.MoviesActors)
					.ThenInclude(aa => aa.Movie)
				.ToList());
		}

		public ActorDto GetActor(int id)
		{
			return _mapper.Map<Actor, ActorDto>(_context.Actors
				.Include(a => a.MoviesActors)
					.ThenInclude(aa => aa.Movie)
				.FirstOrDefault(actor => actor.Id == id));
		}

		public ActorDto UpdateActor(ActorDto actorDto)
		{
			if (actorDto.Id == null)
			{
				return null;
			}

			try
			{
				var actor = _mapper.Map<Actor>(actorDto);

				_context.Update(actor);
				_context.SaveChanges();

				return _mapper.Map<ActorDto>(actor);
			}
			catch (DbUpdateException)
			{
				if (!ActorExists((int)actorDto.Id))
					return null;
				else
					return null;
			}
		}

		public ActorDto DeleteActor(int id)
		{
			var actor = _context.Actors.Find(id);

			if (actor == null)
				return null;

			_context.Actors.Remove(actor);
			_context.SaveChanges();

			return _mapper.Map<ActorDto>(actor);
		}

		public ActorDto AddActor(ActorDto actorDto)
		{
			var actor = _context.Add(_mapper.Map<Actor>(actorDto)).Entity;
			_context.SaveChanges();

			return _mapper.Map<ActorDto>(actor);
		}

		private bool ActorExists(int id)
		{
			return _context.Actors.Any(e => e.Id == id);
		}
	}
}
