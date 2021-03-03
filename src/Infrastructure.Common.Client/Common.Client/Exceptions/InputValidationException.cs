using System;
using System.Collections.Generic;

namespace ProData.Infrastructure.Common.Client.Exceptions
{
	public class InputValidationException : Exception
	{
		#region Ctors

		public InputValidationException()
		{ }

		public InputValidationException(string msg) : base(msg)
		{ }

		public InputValidationException(string msg, IEnumerable<string> errors) : base(msg)
		{
			this.Errors.AddRange(errors);
		}

		public InputValidationException(string msgFormat, params object[] args) : base(string.Format(msgFormat, args))
		{ }

		public InputValidationException(string msg, Exception innerException) : base(msg, innerException)
		{ }

		public InputValidationException(int code, string msg) : base(msg)
		{
			this.Code = code;
		}

		public InputValidationException(int code, string msg, Exception innerException) : base(msg, innerException)
		{
			this.Code = code;
		}

		#endregion

		#region Properties

		public int Code { get; set; }
		public List<string> Errors { get; } = new List<string>();
		public string FullMessage
		{
			get
			{
				if (this.Errors.Count == 0)
					return this.Message;

				var errorsStr = string.Join(Environment.NewLine, this.Errors);
				return $"{this.Message}{Environment.NewLine}{errorsStr}";
			}
		}

		#endregion
	}
}