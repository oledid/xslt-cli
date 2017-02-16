using System;

namespace CLI
{
	public class XslTransformResult
	{
		public readonly bool IsSuccessful;
		public readonly Exception Exception;
		public readonly TimeSpan Duration;
		public readonly string Result;

		private XslTransformResult(bool success, Exception exception, TimeSpan duration, string result)
		{
			IsSuccessful = success;
			Exception = exception;
			Duration = duration;
			Result = result;
		}

		public static XslTransformResult Success(TimeSpan duration, string result)
		{
			return new XslTransformResult(success: true, exception: null, duration: duration, result: result);
		}

		public static XslTransformResult Failure(TimeSpan duration, Exception exception)
		{
			return new XslTransformResult(success: false, exception: exception, duration: duration, result: null);
		}
	}
}
