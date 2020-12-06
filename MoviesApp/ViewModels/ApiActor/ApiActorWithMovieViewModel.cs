using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class ApiActorWithMovieViewModel
	{
		public ICollection<MoviesActors> Movies { get; set; }
	}
}
