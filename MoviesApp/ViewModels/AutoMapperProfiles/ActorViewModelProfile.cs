using AutoMapper;
using MoviesApp.Services.Dto;
using System.Linq;

namespace MoviesApp.ViewModels.AutoMapperProfiles
{
	public class ActorViewModelProfile : Profile
	{
		public ActorViewModelProfile()
		{
			CreateMap<ActorDto, ActorViewModel>()
				.ForMember(dto => dto.MoviesActor, opt => opt.MapFrom(scr => scr.Movies
					.Select(movie => new InputMovieViewModel
					{
						Id = movie.Id,
						Title = movie.Title,
						Price = movie.Price,
						Genre = movie.Genre,
						ReleaseDate = movie.ReleaseDate
					}).ToList()))
				.ReverseMap();

			CreateMap<ActorDto, EditActorViewModel>().ReverseMap();
			CreateMap<ActorDto, DeleteActorViewModel>().ReverseMap();
			CreateMap<ActorDto, CreateActorViewModel>().ReverseMap();
		}
	}
}
