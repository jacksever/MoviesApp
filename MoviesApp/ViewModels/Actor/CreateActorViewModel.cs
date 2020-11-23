using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels
{
	public class CreateActorViewModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string Town { get; set; }

		[DataType(DataType.Date)]
		public DateTime Birthday { get; set; }
	}
}
