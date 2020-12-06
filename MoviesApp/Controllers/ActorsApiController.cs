using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApp.Controllers
{
	[ApiController]
	[Route("api/actors")]
	public class ActorsApiController : ControllerBase
	{
		private readonly MoviesContext _context;
		private readonly IMapper _mapper;

		public ActorsApiController(MoviesContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ApiActorViewModel>))]
		[ProducesResponseType(404)]
		public ActionResult<IEnumerable<ApiActorViewModel>> GetActors()
		{
			var actors = _mapper.Map<IEnumerable<Actor>, IEnumerable<ApiActorViewModel>>(_context.Actors
				.Include(a => a.MoviesActors)
					.ThenInclude(a => a.Movie)
				.ToList());

			return Ok(actors);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ApiActorViewModel>))]
		[ProducesResponseType(404)]
		public IActionResult GetById(long id)
		{
			var actor = _mapper.Map<IEnumerable<Actor>, IEnumerable<ApiActorViewModel>>(_context.Actors
				.Where(a => a.Id == id)
				.Include(a => a.MoviesActors)
					.ThenInclude(a => a.Movie)
				.ToList());

			if (actor == null) return NotFound();
			
			return Ok(actor);
		}

		[HttpPost]
		public ActionResult<ApiCreateActorViewModel> AddActor(ApiCreateActorViewModel createModel)
		{
			var actor = _context.Add(_mapper.Map<Actor>(createModel)).Entity;
			_context.SaveChanges();

			return CreatedAtAction("GetById", new { id = actor.Id }, _mapper.Map<ApiCreateActorViewModel>(createModel));
		}

		[HttpPut("{id}")]
		public IActionResult UpdateActor(int id, ApiUpdateActorViewModel updateModel)
		{
			try
			{
				var actor = _mapper.Map<Actor>(updateModel);
				actor.Id = id;

				_context.Update(actor);
				_context.SaveChanges();

				return Ok(_mapper.Map<ApiUpdateActorViewModel>(actor));
			}
			catch (DbUpdateException)
			{
				if (!ActorsExists(id))
					return BadRequest();
				else
					throw;
			}
		}

		[HttpDelete("{id}")]
		public ActionResult<ApiDeleteActorViewModel> DeleteActor(int id)
		{
			var actor = _context.Actors.Find(id);
			if (actor == null) return NotFound();
			_context.Actors.Remove(actor);
			_context.SaveChanges();

			return Ok(_mapper.Map<ApiDeleteActorViewModel>(actor));
		}

		private bool ActorsExists(long id)
		{
			return _context.Actors.Any(a => a.Id == id);
		}
	}
}
