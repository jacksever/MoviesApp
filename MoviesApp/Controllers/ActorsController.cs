using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Filters;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;
using System.Collections.Generic;

namespace MoviesApp.Controllers
{
	public class ActorsController : Controller
	{
		private readonly IActorService _service;
		private readonly IActorWithMovieService _serviceWithMovie;
		private readonly IMapper _mapper;

		public ActorsController(IActorService service, IActorWithMovieService serviceWithMovie, IMapper mapper)
		{
			_service = service;
			_serviceWithMovie = serviceWithMovie;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View(_mapper.Map<IEnumerable<ActorDto>, IEnumerable<ActorViewModel>>(_service.GetAllActor()));
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
				_service.AddActor(_mapper.Map<ActorDto>(inputModel));

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

			var viewModel = _mapper.Map<ActorViewModel>(_service.GetActor((int)id));


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

			var editModel = _mapper.Map<EditActorViewModel>(_service.GetActor((int)id));

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
				var actor = _mapper.Map<ActorDto>(editModel);
				actor.Id = id;

				var result = _service.UpdateActor(actor);

				if (result == null)
					return NotFound();

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

			var deleteModel = _mapper.Map<DeleteActorViewModel>(_service.GetActor((int)id));

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
			var actor = _service.DeleteActor(id);

			if (actor == null)
				return NotFound();

			return RedirectToAction(nameof(Index));
		}

		[HttpPost, ActionName("Details")]
		[ValidateAntiForgeryToken]
		public IActionResult Detach(int id, int movieId)
		{
			var result = _serviceWithMovie.Detach(id, movieId);

			if (!result)
				return BadRequest();

			return RedirectToAction(nameof(Details));
		}
	}
}
