using Microsoft.AspNetCore.Builder;

namespace MoviesApp.Middleware
{
	public static class RequestLogMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestLog(this IApplicationBuilder applicationBuilder)
		{
			return applicationBuilder.UseMiddleware<RequestLogMiddleware>();
		}
	}
}
