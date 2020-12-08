using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class ActorViewModel : CreateActorViewModel
	{
		public int Id { get; set; }
		public virtual ICollection<InputMovieViewModel> MoviesActor { get; set; }
	}
}
