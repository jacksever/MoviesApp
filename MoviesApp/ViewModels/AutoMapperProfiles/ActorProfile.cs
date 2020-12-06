using AutoMapper;
using MoviesApp.Models;
using System.Linq;

namespace MoviesApp.ViewModels.AutoMapperProfiles
{
	public class ActorProfile : Profile
	{
		public ActorProfile()
		{
			CreateMap<Actor, ApiActorViewModel>()
				.ForMember(dto => dto.Movies, opt => opt.MapFrom(scr => scr.MoviesActors
					.Select(movie => new ApiMovieViewModel
					{
						Title = movie.Movie.Title,
						Price = movie.Movie.Price,
						Genre = movie.Movie.Genre,
						ReleaseDate = movie.Movie.ReleaseDate
					}).ToList()))
				.ReverseMap();

			CreateMap<Actor, ApiActorWithMovieViewModel>()
				.ForMember(dto => dto.Movies, opt => opt.MapFrom(scr => scr.MoviesActors))
				.ReverseMap();

			CreateMap<Actor, ApiCreateActorViewModel>().ReverseMap();
			CreateMap<Actor, ApiUpdateActorViewModel>().ReverseMap();
			CreateMap<Actor, ApiDeleteActorViewModel>().ReverseMap();
		}
	}
}