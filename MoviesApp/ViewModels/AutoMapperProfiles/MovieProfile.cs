using AutoMapper;
using MoviesApp.Models;

namespace MoviesApp.ViewModels.AutoMapperProfiles
{
	public class MovieProfile : Profile
	{
		public MovieProfile()
		{
			CreateMap<Movie, ApiMovieWithActorViewModel>()
				.ForMember(dto => dto.Actors, opt => opt.MapFrom(scr => scr.MoviesActors))
				.ReverseMap();

			CreateMap<Movie, ApiMovieByIdViewModel>().ReverseMap();
		}
	}
}
