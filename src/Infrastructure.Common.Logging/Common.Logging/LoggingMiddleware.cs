using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace ProData.Infrastructure.Common.Logging
{
	public class LoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public LoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			using (LogContext.PushProperty("UserName", context.User.Identity?.Name ?? "anonymous"))
			using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier ?? Guid.NewGuid().ToString()))
			{
				await _next(context);
			}
		}
	}
}
