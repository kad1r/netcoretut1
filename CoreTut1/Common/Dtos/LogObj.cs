using Common.Enums;
using System;
using System.Collections.Generic;

namespace Common.Dtos
{
	public class LogObj
	{
		public LogType LogType { get; set; }
		public DateTime DateInserted { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string RouteDataId { get; set; }
		public string User { get; set; }
		public string Message { get; set; }
		public string ExceptionMessage { get; set; }
		public string InnerExceptionSource { get; set; }
		public string InnerExceptionMessage { get; set; }
		public string InnerExceptionStackTrace { get; set; }
		public Dictionary<string, string> arrObject { get; set; }
	}
}