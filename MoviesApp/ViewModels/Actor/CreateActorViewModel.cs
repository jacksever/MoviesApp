using MoviesApp.Filters;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
	public class CreateActorViewModel
	{
		[Required]
		[CheckNameActorsFilter]
		public string FirstName { get; set; }

		[Required]
		[CheckNameActorsFilter]
		public string LastName { get; set; }

		[Required]
		public int Age { get; set; }

		[Required]
		public string Town { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Birthday { get; set; }
	}
}
