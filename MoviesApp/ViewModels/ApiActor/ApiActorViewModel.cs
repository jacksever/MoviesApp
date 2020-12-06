using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class ApiActorViewModel : CreateActorViewModel
	{
		public int Id { get; set; }

		public List<ApiMovieViewModel> Movies { get; set; }
	}
}
