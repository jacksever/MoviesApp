using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
	public class ActorViewModel : CreateActorViewModel
	{
		public int Id { get; set; }
		public virtual ICollection<MoviesActors> Movies { get; set; }
	}
}
