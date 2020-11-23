using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
	public class Actor
	{
		public Actor()
		{
			this.MoviesActors = new HashSet<MoviesActors>();
		}

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string Town { get; set; }

		[DataType(DataType.Date)]
		public DateTime Birthday { get; set; }

		public virtual ICollection<MoviesActors> MoviesActors { get; set; }
	}
}
