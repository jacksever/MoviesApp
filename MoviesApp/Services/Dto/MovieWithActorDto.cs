using MoviesApp.Models;
using System.Collections.Generic;

namespace MoviesApp.Services.Dto
{
	public class MovieWithActorDto
	{
		public virtual ICollection<MoviesActors> MoviesActors { get; set; }
	}
}
