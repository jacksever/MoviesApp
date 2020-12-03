using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Filters
{
	public class CheckNameActorsFilter : ValidationAttribute
	{
		private string GetErrorMessage() => "Actor's name and surname must be more than 4 characters long!";

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value.ToString().Length < 4)
				return new ValidationResult(GetErrorMessage());

			return ValidationResult.Success;
		}
	}
}
