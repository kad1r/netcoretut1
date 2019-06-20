using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace Common.Helpers
{
	public class ReflectionHelper<T> where T : class
	{
		/// <summary>
		/// Gönderilen objeyi birebir kopyalar, destination uzerinde ne varsa overwrite olur.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public static T CopyObject(object source, T destination)
		{
			var props = source.GetType().GetProperties();

			foreach (var prop in props)
			{
				var info = destination.GetType()
					.GetProperty(prop.Name);

				if (info != null)
				{
					info.SetValue(destination, prop.GetValue(source, null), null);
				}
			}

			return destination;
		}

		/// <summary>
		/// Gonderilen listeyi datatable olarak geri doner.
		/// Header kismi dahil degildir.
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		public static DataTable ToDataTable(List<T> items)
		{
			var dataTable = new DataTable(typeof(T).Name);

			var Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo prop in Props)
			{
				dataTable.Columns.Add(prop.Name);
			}

			foreach (T item in items)
			{
				var values = new object[Props.Length];

				for (int i = 0; i < Props.Length; i++)
				{
					values[i] = Props[i].GetValue(item, null);
				}

				dataTable.Rows.Add(values);
			}

			return dataTable;
		}

		public static string GetPropertyDisplayValue(string propertyName)
		{
			var property = typeof(T).GetProperty(propertyName);

			return property != null ? (property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute).Name : propertyName;
		}
	}
}