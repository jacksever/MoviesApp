using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using System.Collections.Generic;

namespace MoviesApp.Controllers
{
	[ApiController]
	[Route("api/actors")]
	public class ActorsApiController : ControllerBase
	{
		private readonly IActorService _service;

		public ActorsApiController(IActorService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ActorDto>))]
		[ProducesResponseType(404)]
		public ActionResult<IEnumerable<ActorDto>> GetActors()
		{
			var actors = _service.GetAllActor();

			return Ok(actors);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(200, Type = typeof(ActorDto))]
		[ProducesResponseType(404)]
		public IActionResult GetById(int id)
		{
			var actor = _service.GetActor(id);

			if (actor == null) return NotFound();

			return Ok(actor);
		}

		[HttpPost]
		public ActionResult<ActorDto> AddActor(ActorDto createModel)
		{
			var actor = _service.AddActor(createModel);

			return CreatedAtAction("GetById", new { id = actor.Id }, actor);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateActor(int id, ActorDto updateModel)
		{
			updateModel.Id = id;
			var actor = _service.UpdateActor(updateModel);

			if (actor == null)
				return BadRequest();

			return Ok(actor);
		}

		[HttpDelete("{id}")]
		public ActionResult<ActorDto> DeleteActor(int id)
		{
			var actor = _service.DeleteActor(id);
			if (actor == null) return NotFound();

			return Ok(actor);
		}
	}
}
