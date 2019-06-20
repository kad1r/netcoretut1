using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Helpers
{
	public static class ContextHelper
	{
		/// <summary>
		/// Returns full url of the page as a string
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string GetFullUrl(this HttpRequest request)
		{
			return Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);
		}


		public static Uri GetUri(this HttpRequest request)
		{
			var uriBuilder = new UriBuilder
			{
				Scheme = request.Scheme,
				Host = request.Host.Host,
				Port = request.Host.Port.GetValueOrDefault(80),
				Path = request.Path.ToString(),
				Query = request.QueryString.ToString()
			};

			return uriBuilder.Uri;
		}


		/// <summary>
		/// Returns http referrer from request object
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string GetReferrer(this HttpRequest request)
		{
			return request.Headers["Referer"].ToString();
		}


		public static string GetQueryParameters(this HttpRequest request)
		{
			var parameters = new System.Dynamic.ExpandoObject();
			var query = request.Query;
			var referrer = GetReferrer(request);
			var keys = query.Keys.ToList();
			var refKeys = new System.Collections.Specialized.NameValueCollection();

			if (query.Keys != null && query.Keys.Count > 0)
			{

			}
			else
			{
				// TODO
				// check keys for the list type if they are same for querystring and referer
				// parse query parameters to expandoobject

				if (!string.IsNullOrWhiteSpace(referrer))
				{
					var uri = new Uri(referrer);
					keys = HttpUtility.ParseQueryString(uri.Query).AllKeys.ToList();
				}

				// checking referrer link and parse referer querystring if it's exist
				//var referer = request.Headers["referer"];

				//if (!string.IsNullOrWhiteSpace(referer))
				//{
				//	refKeys = HttpUtility.ParseQueryString(referer);
				//}
			}

			var referer = request.Headers["referer"];

			if (!string.IsNullOrWhiteSpace(referer))
			{
				refKeys = HttpUtility.ParseQueryString(referer, Encoding.UTF8);
			}

			foreach (var key in keys)
			{

			}

			return string.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static string GetUrl(ActionExecutingContext context)
		{
			return Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedPathAndQuery(context.HttpContext.Request).Split('?')[1];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static Dictionary<string, Microsoft.Extensions.Primitives.StringValues> GetQuery(string url)
		{
			return Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(url);
		}
	}
}