using System.Globalization;

namespace Common.Helpers
{
	public static class StringHelper
	{
		public static string FormatNumber(int num)
		{
			return num.ToString("#,###", CultureInfo.InvariantCulture);
		}
	}
}
