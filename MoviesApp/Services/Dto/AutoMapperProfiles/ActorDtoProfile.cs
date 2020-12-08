using AutoMapper;
using MoviesApp.Models;
using System.Linq;

namespace MoviesApp.Services.Dto.AutoMapperProfiles
{
	public class ActorDtoProfile : Profile
	{
		public ActorDtoProfile()
		{
			CreateMap<Actor, ActorDto>()
				.ForMember(dto => dto.Movies, opt => opt.MapFrom(scr => scr.MoviesActors
					.Select(movie => new MovieDto
					{
						Id = movie.MovieId,
						Title = movie.Movie.Title,
						Price = movie.Movie.Price,
						Genre = movie.Movie.Genre,
						ReleaseDate = movie.Movie.ReleaseDate
					}).ToList()))
				.ReverseMap();
		}
	}
}
