using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApp.Controllers
{
	public class HelloWorldController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public string Welcome(string name, int numTimes = 1)
		{
			return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
		}
	}
}