using Common.Dtos;
using Common.Enums;
using Common.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace Common.Filters
{
	public class ExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			Trace.TraceError(filterContext.Exception.ToString());

			if (filterContext.Exception.InnerException != null)
			{
				Trace.TraceError(filterContext.Exception.InnerException.ToString());
			}

			var log = new LogObj
			{
				DateInserted = DateTime.Now,
				LogType = LogType.Exception,
				Controller = filterContext.RouteData != null ? (string)filterContext.RouteData.Values["controller"] : "",
				Action = filterContext.RouteData != null ? (string)filterContext.RouteData.Values["action"] : "",
				RouteDataId = filterContext.RouteData != null ? (string)filterContext.RouteData.Values["id"] : "",
				User = "",
				Message = filterContext.Exception.ToString(),
				ExceptionMessage = filterContext.Exception.Message,
				InnerExceptionMessage = filterContext.Exception.InnerException != null ? filterContext.Exception.InnerException.ToString() : "",
				InnerExceptionSource = filterContext.Exception.Source,
				InnerExceptionStackTrace = filterContext.Exception.StackTrace
			};

			LogHelper.WriteLog(LogTarget.Exception, log);
		}
	}
}