using System;
using ProData.Infrastructure.Common.Exceptions;

namespace ProData.Infrastructure.Common.Client.Exceptions
{
	public class ApiException : PdException
	{
		public ApiException()
		{
		}

		public ApiException(string msg)
			: base(msg)
		{
		}

		public ApiException(string msgFormat, params object[] args)
			: base(string.Format(msgFormat, args))
		{
		}

		public ApiException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		public ApiException(int code, string msg)
			: base(msg)
		{
			this.Code = code;
		}

		public ApiException(int code, string msg, Exception innerException)
			: base(msg, innerException)
		{
			this.Code = code;
		}
	}
}