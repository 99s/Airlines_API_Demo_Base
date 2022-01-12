using Microsoft.AspNetCore.Builder;

using AD_API_CORE.Middleware;

namespace AD_API_CORE.Extensions
{
	public static class ExceptionHandlerMiddlewareExtension
	{
		public static IApplicationBuilder UseSimpleExceptionHandler(this IApplicationBuilder builder) =>
			builder.UseMiddleware<SimpleExceptionHandlerMiddleware>();
	}
}
