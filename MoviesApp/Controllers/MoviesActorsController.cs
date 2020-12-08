using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
	public class MoviesActorsController : Controller
	{
		private readonly IActorWithMovieService _service;

		public MoviesActorsController(IActorWithMovieService service)
		{
			_service = service;
		}

		[HttpGet]
		public IActionResult AddActor(int? id)
		{
			if (id == null)
				return BadRequest();

			var list = _service.AddActor((int)id);

			if (list == null)
				return NotFound();

			return View(list);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddActor(int id, int actorId)
		{
			var result = _service.Attach(actorId, id);

			if (!result)
				return BadRequest();

			return RedirectToAction(nameof(AddActor));
		}

		[HttpGet]
		public IActionResult AddMovie(int? id)
		{
			if (id == null)
				return BadRequest();

			var list = _service.AddMovie((int)id);

			if (list == null)
				return NotFound();

			return View(list);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddMovie(int id, int movieId)
		{
			var result = _service.Attach(id, movieId);

			if (!result)
				return BadRequest();

			return RedirectToAction(nameof(AddMovie));
		}
	}
}
