using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class ApiMovieWithActorViewModel
	{
		public ICollection<MoviesActors> Actors { get; set; }
	}
}
