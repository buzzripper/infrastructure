using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProData.Infrastructure.Common.Client.Api;
using ProData.Infrastructure.Common.Client.Exceptions;
using ProData.Infrastructure.Common.Client.Extensions;
using ProData.Infrastructure.Common.Extensions;

namespace ProData.Infrastructure.Common.Server.Controllers
{
	public abstract class ApiControllerBase : Controller
	{
		[HttpGet, Route("[action]")]
		[AllowAnonymous]
		public PingResult Ping()
		{
			return new PingResult { UtcTime = DateTime.UtcNow };
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected IActionResult CallService(Action action)
		{
			var apiResponse = new ApiResponse();
			try {
				action();
				apiResponse.StatusCode = 200;
				return Ok(apiResponse);

			} catch (InputValidationException ex) {
				return BadRequest(new ApiResponse(400, ex.Message, ex.Errors));

			} catch (Exception ex) {
                this.LogError(new StackFrame(1).GetMethod().DeclaringType?.Name, ex.GetInnermostException());

				apiResponse.StatusCode = 500;
				apiResponse.Message = "Unexpected server error.";
				return StatusCode(500, apiResponse);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected async Task<IActionResult> CallServiceAsync(Func<Task> func)
		{
			var apiResponse = new ApiResponse();
			try {
				await func();
				apiResponse.StatusCode = 200;
				return Ok(apiResponse);

			} catch (Exception ex) {
				var innermostEx = ex.GetInnermostException();
				
				if (innermostEx is InputValidationException) {
					return BadRequest(new ApiResponse(400, innermostEx.Message));

				} else {
                    this.LogError(new StackFrame(1).GetMethod().DeclaringType?.Name, innermostEx);

					apiResponse.StatusCode = 500;
					apiResponse.Message = "Unexpected server error.";
					return StatusCode(500, apiResponse);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected IActionResult CallService<T>(Func<T> func)
		{
			var apiResponse = new ApiResponse<T>();
			try {
				apiResponse.Data = func();
				apiResponse.StatusCode = 200;
				return Ok(apiResponse);

			} catch (Exception ex) {
				var innermostEx = ex.GetInnermostException();
				
				if (innermostEx is InputValidationException) {
					return BadRequest(new ApiResponse(400, innermostEx.Message));

				} else {
                    this.LogError(new StackFrame(1).GetMethod().DeclaringType?.Name, innermostEx);

					apiResponse.StatusCode = 500;
					apiResponse.Message = "Unexpected server error.";
					return StatusCode(500, apiResponse);
				}
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected async Task<IActionResult> CallServiceAsync<T>(Func<Task<T>> func)
		{
			var apiResponse = new ApiResponse<T>();
			try {
				apiResponse.Data = await func();
				apiResponse.StatusCode = 200;
				return Ok(apiResponse);

			} catch (Exception ex) {
				var innermostEx = ex.GetInnermostException();
				
				if (innermostEx is InputValidationException) {
					return BadRequest(new ApiResponse(400, innermostEx.Message));

				} else {
					this.LogError(new StackFrame(1).GetMethod().DeclaringType?.Name, innermostEx);

					apiResponse.StatusCode = 500;
					apiResponse.Message = "Unexpected server error.";
					return StatusCode(500, apiResponse);
				}
			}
		}

		private void LogError(string callingClassName, Exception ex)
        {
            Serilog.Log.Logger.ForContext(Serilog.Core.Constants.SourceContextPropertyName, callingClassName);

            //var nlogLogger = LogManager.GetCurrentClassLogger();

            //LogEventInfo theEvent = new LogEventInfo(NLog.LogLevel.Debug, "apiLogger", $"{ex.GetType().Name}: {ex}");
            //theEvent.Properties["className"] = callingClassName;
            //theEvent.Properties["methodName"] = callingMethodName;

            //nlogLogger.Log(theEvent);
        }
	}
}