using AppContext = Common.Base.AppContext;

namespace Common.Helpers
{
	public static class CookieHelper
	{
		public static void Add(string key, string value)
		{
			AppContext.Current.Response.Cookies.Append(key, value);
		}

		public static string Get(string key)
		{
			return AppContext.Current.Request.Cookies[key];
		}

		public static void Remove(string key)
		{
			AppContext.Current.Response.Cookies.Delete(key);
		}
	}
}