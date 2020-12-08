using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using System.Collections.Generic;

namespace MoviesApp.Controllers
{
	[ApiController]
	[Route("api/moviesActors")]
	public class MoviesActorsApiController : ControllerBase
	{
		private readonly IActorWithMovieService _service;

		public MoviesActorsApiController(IActorWithMovieService service)
		{
			_service = service;
		}

		[HttpGet("addActor/{id}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ActorDto>))]
		[ProducesResponseType(404)]
		public ActionResult<IEnumerable<ActorDto>> AddActor(int? id)
		{
			if (id == null)
				return BadRequest();

			var actorList = _service.AddActor((int)id);

			if (actorList == null)
				return NotFound();

			return Ok(actorList);

		}

		[HttpPost("addActor/{movieId}/{actorId}")]
		public IActionResult AddActor(int movieId, int actorId)
		{
			var result = _service.Attach(actorId, movieId);

			if (!result)
				return BadRequest("The selected movie already has this actor!");

			return Ok("The actor was successfully added to this movie :)");
		}

		[HttpGet("addMovie/{id}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<MovieDto>))]
		[ProducesResponseType(404)]
		public ActionResult<MovieDto> AddMovie(int? id)
		{
			if (id == null)
				return NotFound();

			var movieList = _service.AddMovie((int)id);

			if (movieList == null)
				return NotFound();

			return Ok(movieList);
		}

		[HttpPost("addMovie/{actorId}/{movieId}")]
		public IActionResult AddMovie(int actorId, int movieId)
		{
			var result = _service.Attach(actorId, movieId);

			if (!result)
				return BadRequest("The selected actor already has this movie!");

			return Ok("This movie is successfully attached to this actor");
		}


		[HttpDelete("detach/{actorId}/{movieId}")]
		public IActionResult Detach(int actorId, int movieId)
		{
			var result = _service.Detach(actorId, movieId);

			if (!result)
				return BadRequest("The actor or the film is not attached or not found!");

			return Ok("Actor and movie successfully detached from each other :)");
		}
	}
}
