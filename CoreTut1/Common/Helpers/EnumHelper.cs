using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Helpers
{
	public static class EnumHelper
	{
		public static IEnumerable<SelectListItem> ToSelectList(this Enum enumValue)
		{
			return null;
			/*
			return from Enum e in Enum.GetValues(enumValue.GetType())
				   select new SelectListItem
				   {
					   Selected = e.Equals(enumValue),
					   Text = e.ToDescription(),
					   Value = e.ToString()
				   };
				   */
		}

		public static string ToDescription(this Enum value)
		{
			var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attributes.Length > 0 ? attributes[0].Description : value.ToString();
		}

		public static string getVal(this Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				return attributes[0].Description;
			}
			else
			{
				return value.ToString();
			}
		}

		public static int GetValueOf(string enumName, string enumConst)
		{
			var enumType = Type.GetType(enumName);
			if (enumType == null)
			{
			}

			var value = Enum.Parse(enumType, enumConst);
			return Convert.ToInt32(value);
		}

		public static string GetDisplayValue(Type type, int enumConst)
		{
			var enumValue = Enum.GetName(type, enumConst);
			var member = type.GetMember(enumValue)[0];

			var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
			var outString = ((DisplayAttribute)attrs[0]).Name;

			if (((DisplayAttribute)attrs[0]).ResourceType != null)
			{
				outString = ((DisplayAttribute)attrs[0]).GetName();
			}

			return outString;
		}

		public static IEnumerable<string> GetDescriptions(Type type)
		{
			var descs = new List<string>();
			var names = Enum.GetNames(type);

			foreach (var name in names)
			{
				var field = type.GetField(name);
				var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
				foreach (DescriptionAttribute fd in fds)
				{
					descs.Add(fd.Description);
				}
			}

			return descs;
		}

		public static IEnumerable<string> GetNames(Type type)
		{
			var nameList = new List<string>();
			var names = Enum.GetNames(type);

			foreach (var name in names)
			{
				var field = type.GetField(name);
				var fds = field.GetCustomAttributes(typeof(DisplayAttribute), true);
				foreach (DisplayAttribute fd in fds)
				{
					nameList.Add(fd.Name);
				}
			}

			return nameList;
		}

		public static Dictionary<int, string> GetNamesWithValues(Type type)
		{
			var list = new Dictionary<int, string>();
			var values = Enum.GetValues(type);

			foreach (var value in values)
			{
				var field = type.GetField(value.ToString());
				var f = field.GetCustomAttributes(typeof(DisplayAttribute), true);

				foreach (DisplayAttribute a in f)
				{
					if (a.ResourceType.Name == "Resources")
					{
						list.Add((int)value, a.Name);
					}
					else
					{
						list.Add((int)value, a.Name);
					}
				}
			}

			return list;
		}

		public static Dictionary<int, string> GetDescriptionWithValues(Type type)
		{
			var list = new Dictionary<int, string>();

			var values = Enum.GetValues(type);

			foreach (var value in values)
			{
				var field = type.GetField(value.ToString());
				var f = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
				foreach (DescriptionAttribute a in f)
				{
					list.Add((int)value, a.Description);
				}
			}

			return list;
		}

		public static IEnumerable<SelectListItem> EnumToSelectList(Type type, string attribute)
		{
			var list = new List<SelectListItem>();
			var dict = new Dictionary<int, string>();

			if (attribute.ToLower() == "name")
				dict = GetNamesWithValues(type);

			if (attribute.ToLower() == "description")
				dict = GetDescriptionWithValues(type);

			foreach (var item in dict)
			{
				list.Add(new SelectListItem { Text = item.Value, Value = item.Key.ToString() });
			}

			return list;
		}
	}
}