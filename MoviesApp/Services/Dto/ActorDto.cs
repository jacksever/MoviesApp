using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Services.Dto
{
	public class ActorDto
	{
		public int? Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string Town { get; set; }

		[DataType(DataType.Date)]
		public DateTime Birthday { get; set; }

		public virtual ICollection<MovieDto> Movies { get; set; }
	}
}
