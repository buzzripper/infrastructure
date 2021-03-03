using System.Collections.Generic;

namespace ProData.Infrastructure.Common.Client.Api
{
	public class ApiResponse
	{
		public ApiResponse()
		{
		}

		public ApiResponse(int statusCode, string message = null, IEnumerable<string> errors = null)
		{
			this.StatusCode = statusCode;
			this.Message = message;
			this.Errors = errors;
		}

		public int StatusCode { get; set; }
		public string Message { get; set; }
		public IEnumerable<string> Errors { get; } = new List<string>();
		public string CorrelationId { get; set; }
	}

	public class ApiResponse<T> : ApiResponse
	{
		public ApiResponse()
		{
		}

		public ApiResponse(int statusCode, string message = null) 
			: base(statusCode, message)
		{
		}

		public ApiResponse(T data)
			: this(200)
		{
			this.Data = data;
		}

		public T Data { get; set; }
	}

}