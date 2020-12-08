using MoviesApp.Services.Dto;
using System.Collections.Generic;

namespace MoviesApp.Services
{
	public interface IActorService
	{
		IEnumerable<ActorDto> GetAllActor();
		ActorDto GetActor(int id);
		ActorDto UpdateActor(ActorDto actorDto);
		ActorDto DeleteActor(int id);
		ActorDto AddActor(ActorDto actorDto);
	}
}
