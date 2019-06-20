using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Common.Base
{
	public static class AppContext
	{
		private static IHttpContextAccessor _httpContextAccessor;
		public static IHostingEnvironment env { get; set; }

		public static void Configure(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public static HttpContext Current => _httpContextAccessor.HttpContext;
	}
}