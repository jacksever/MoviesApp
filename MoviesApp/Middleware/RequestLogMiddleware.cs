using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MoviesApp.Middleware
{
	public class RequestLogMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestLogMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task Invoke(HttpContext httpContext, ILogger<RequestLogMiddleware> logger)
		{
			if (httpContext.Request.Path.Value.Contains("Actors"))
				logger.LogTrace($"Request: {httpContext.Request.Path} Method: {httpContext.Request.Method}");

			await _next(httpContext);
		}
	}
}
