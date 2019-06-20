using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AppContext = Common.Base.AppContext;

namespace Common.Helpers
{
	public static class MailHelper
	{
		public static string CustomFormat(string data, params object[] parameters)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				var pattern = string.Format(@"(\[{0}\])", i);

				data = Regex.Replace(data, pattern, parameters[i].ToString());
			}

			return data;
		}

		public static string GetMailTemplate(string templateTitle)
		{
			using (var sr = new StreamReader(AppContext.env.ContentRootPath + @"~/Utils/MailTemplates/" + templateTitle + ".html", Encoding.Default))
			{
				return sr.ReadToEnd();
			}
		}
	}
}