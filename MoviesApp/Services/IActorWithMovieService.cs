using MoviesApp.Services.Dto;
using System.Collections.Generic;

namespace MoviesApp.Services
{
	public interface IActorWithMovieService
	{
		public bool Detach(int actorId, int movieId);

		public bool Attach(int actorId, int movieId);

		public IEnumerable<ActorDto> AddActor(int id);

		public IEnumerable<MovieDto> AddMovie(int id);
	}
}
