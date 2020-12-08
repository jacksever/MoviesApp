using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class MovieViewModel : InputMovieViewModel
	{
		public virtual ICollection<MoviesActors> Actors { get; set; }
	}
}