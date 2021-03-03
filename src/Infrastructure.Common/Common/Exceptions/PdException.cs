using System;

namespace ProData.Infrastructure.Common.Exceptions
{
	public class PdException : Exception
	{
		public PdException()
		{
		}

		public PdException(string msg)
			: base(msg)
		{
		}

		public PdException(string msgFormat, params object[] args)
			: base(string.Format(msgFormat, args))
		{
		}

		public PdException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		public PdException(int code, string msg)
			: base(msg)
		{
			this.Code = code;
		}

		public PdException(int code, string msg, Exception innerException)
			: base(msg, innerException)
		{
			this.Code = code;
		}

		public int Code { get; set; }
	}
}