using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MoviesApp.Filters
{
	public class CheckAgeActorsFilter : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context) { }

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var age = int.Parse(context.HttpContext.Request.Form["Age"]);

			if (age < 7 || age > 99)
				context.Result = new RedirectToActionResult("Error", "Home", null);
		}
	}
}
