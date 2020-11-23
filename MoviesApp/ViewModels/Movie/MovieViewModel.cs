using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class MovieViewModel : InputMovieViewModel
	{
		public int Id { get; set; }

		public virtual ICollection<MoviesActors> Actors { get; set; }
	}
}