using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
	public class CommonHelper
	{
		public static void GridExportToExcel(string fileName, object dataTable)
		{
			//using (var grid = new GridView())
			//{
			//	grid.DataSource = dataTable;
			//	grid.DataBind();

			//	if (grid.Rows.Count > 0)
			//	{
			//		var header = grid.HeaderRow;

			//		foreach (TableCell item in header.Cells)
			//		{
			//			//item.Text = ResourceHelper.GetResourceByFieldName(item.Text);
			//		}

			//		foreach (GridViewRow row in grid.Rows)
			//		{
			//			for (int i = 0; i < row.Cells.Count; i++)
			//			{
			//				var cellText = row.Cells[i].Text;

			//				#region DECIMAL KONTROLU

			//				// gelen deger fiyat ise ve decimal'e parse oluyorsa string degerini excel'e yazarken noktayi virgule cevirerek yaziyoruz
			//				// aksi halde 2.69 rakamini subat.69 olarak algiliyor.

			//				decimal numberResult = 0;
			//				decimal.TryParse(cellText, out numberResult);

			//				if (numberResult != 0)
			//				{
			//					row.Cells[i].Text = String.Format("{0:F2}", cellText.Replace(".", ","));
			//				}

			//				double doubleResult = 0;
			//				double.TryParse(cellText, out doubleResult);

			//				if (doubleResult != 0)
			//				{
			//					row.Cells[i].Text = String.Format("{0:F2}", cellText.Replace(".", ","));
			//				}

			//				#endregion DECIMAL KONTROLU

			//				#region TARIH KONTROLU

			//				var dt = DateTime.MinValue;

			//				if (DateTime.TryParse(cellText, out dt))
			//				{
			//					if (dt == DateTime.MinValue)
			//					{
			//						row.Cells[i].Text = "";
			//					}
			//				}

			//				#endregion TARIH KONTROLU
			//			}
			//		}

			//		HttpContext.Current.Response.ClearContent();
			//		HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}-{1}.xls", fileName, DateTime.Now.ToShortDateString().Replace(".", "")));
			//		HttpContext.Current.Response.ContentType = "application/ms-excel";
			//		HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
			//		HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

			//		var sw = new StringWriter();
			//		var htw = new HtmlTextWriter(sw);

			//		grid.RenderControl(htw);

			//		HttpContext.Current.Response.Write(sw.ToString());
			//		HttpContext.Current.Response.End();
			//	}
			//	else
			//	{
			//		HttpContext.Current.Response.Clear();
			//		HttpContext.Current.Response.Write("<script>window.close();</script>");
			//		HttpContext.Current.Response.Flush();
			//		HttpContext.Current.Response.End();
			//	}
			//}
		}

		public static bool GridExportToExcelwithReturn(string fileName, object dataTable)
		{
			try
			{
				GridExportToExcel(fileName, dataTable);
				return true;
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
				return false;
			}
		}

		public static bool DeleteFile(string filepath)
		{
			try
			{
				if (!string.IsNullOrEmpty(filepath))
				{
					if (!filepath.Contains("~"))
					{
						filepath = "~" + filepath;
					}

					//var oldfile = new FileInfo(HttpContext.Current.Server.MapPath(filepath));

					//if (oldfile.Exists)
					//{
					//	oldfile.Delete();
					//	return true;
					//}
				}
			}
			catch (Exception)
			{
				return false;
			}

			return false;
		}

		public static string[] SupportFileType()
		{
			var fileType = new string[] { ".jpeg", ".jpg", ".png", ".bmp", ".doc", ".docx", ".pdf", ".xls", "xlsx", ".xml", ".txt" };

			return fileType;
		}

		/// <summary>
		/// Gönderilen rakamı yazıya çevirir.
		/// </summary>
		/// <param name="tutar"></param>
		/// <returns></returns>
		public static string AmountConverttoString(decimal tutar)
		{
			var sTutar = tutar.ToString("F2").Replace('.', ','); // Replace('.',',') ondalık ayracının . olma durumu için
			var lira = sTutar.Substring(0, sTutar.IndexOf(',')); //tutarın tam kısmı
			var kurus = sTutar.Substring(sTutar.IndexOf(',') + 1, 2);
			var yazi = "";
			string[] birler = { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
			string[] onlar = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
			string[] binler = { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" }; //KATRİLYON'un önüne ekleme yapılarak artırabilir.
			var grupSayisi = 6; //sayıdaki 3'lü grup sayısı. katrilyon içi 6. (1.234,00 daki grup sayısı 2'dir.)
								//KATRİLYON'un başına ekleyeceğiniz her değer için grup sayısını artırınız.
			string grupDegeri;

			lira = lira.PadLeft(grupSayisi * 3, '0'); //sayının soluna '0' eklenerek sayı 'grup sayısı x 3' basakmaklı yapılıyor.

			for (int i = 0; i < grupSayisi * 3; i += 3) //sayı 3'erli gruplar halinde ele alınıyor.
			{
				grupDegeri = "";

				if (lira.Substring(i, 1) != "0")
					grupDegeri += birler[Convert.ToInt32(lira.Substring(i, 1))] + "YÜZ "; //yüzler

				if (grupDegeri.Trim() == "BİRYÜZ") //biryüz düzeltiliyor.
					grupDegeri = "YÜZ ";

				grupDegeri += onlar[Convert.ToInt32(lira.Substring(i + 1, 1))] + " "; //onlar

				grupDegeri += birler[Convert.ToInt32(lira.Substring(i + 2, 1))] + " "; //birler

				if (grupDegeri.Trim() != "") //binler
					grupDegeri += binler[i / 3] + " ";

				if (grupDegeri.Trim() == "BİRBİN") //birbin düzeltiliyor.
					grupDegeri = "BİN ";

				if (!string.IsNullOrWhiteSpace(grupDegeri)) yazi += grupDegeri;
			}

			if (yazi != "")
				yazi += "TL ";

			var yaziUzunlugu = yazi.Length;

			if (kurus.Substring(0, 1) != "0") //kuruş onlar
				yazi += onlar[Convert.ToInt32(kurus.Substring(0, 1))];

			if (kurus.Substring(1, 1) != "0") //kuruş birler
				yazi += birler[Convert.ToInt32(kurus.Substring(1, 1))];

			if (yazi.Length > yaziUzunlugu)
				yazi += " KR.";
			else
				yazi += "SIFIR KR.";

			return "YALNIZ " + yazi.Replace("  ", " ");
		}

		#region Base64 Encode - Decode

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);

			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		#endregion Base64 Encode - Decode

		#region QUERY HELPERS

		/// <summary>
		/// Gelen query'i parse eder ve querystring elemanlarini dictionary'e atar.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="qs"></param>
		/// <param name="referer"></param>
		/// <returns></returns>
		public static Dictionary<string, string> GetQueryStringItems(string query, NameValueCollection qs = null, Uri referer = null)
		{
			var dic = new Dictionary<string, string>();

			if (!string.IsNullOrWhiteSpace(query))
			{
				query = query.StartsWith("?") ? query.Substring(1, query.Length - 1) : query;

				var queryArray = query.Split('&').ToList();

				if (!string.IsNullOrWhiteSpace(query))
				{
					foreach (var item in queryArray)
					{
						var itemArr = item.Split('=');

						if (itemArr.Length > 1)
						{
							if (!dic.ContainsKey(itemArr[0]))
							{
								dic.Add(itemArr[0], itemArr[1]);
							}
						}
					}
				}
			}
			else if (qs != null && qs.Count > 0)
			{
				foreach (var item in qs.AllKeys)
				{
					if (!dic.ContainsKey(item))
						dic.Add(item, qs[item]);
				}
			}
			else if (referer != null)
			{
				if (referer.Query.Contains("&"))
				{
					var qsParameters = referer.Query.Split('?')
						.Skip(1);

					qsParameters = qsParameters.First()
						.Split('&')
						.ToList();

					foreach (var item in qsParameters)
					{
						var array = item.Split('=');

						if (array.Length > 1)
						{
							dic.Add(array[0], array[1]);
						}
					}
				}
				else
				{
					var qsParameters = referer.Query.Split('?')
						.Skip(1)
						.ToList();
					foreach (var item in qsParameters)
					{
						var array = item.Split('=');

						if (array.Length > 1)
						{
							dic.Add(array[0], array[1]);
						}
					}
				}
			}

			return dic;
		}

		#endregion QUERY HELPERS

		public static string FriendlyClear(string s)
		{
			s = s.ToLower();

			var sb = new StringBuilder(s);
			sb.Replace("ş", "s");
			sb.Replace("Ş", "s");
			sb.Replace("İ", "i");
			sb.Replace("I", "i");
			sb.Replace("ı", "i");
			sb.Replace("ö", "o");
			sb.Replace("Ö", "o");
			sb.Replace("ü", "u");
			sb.Replace("Ü", "u");
			sb.Replace("Ç", "c");
			sb.Replace("ç", "c");
			sb.Replace("ğ", "g");
			sb.Replace("Ğ", "g");

			var res = (sb.ToString() ?? "").ToLower();

			res = Regex.Replace(res, "\\&+", "and");
			res = res.Replace("'", "");
			res = Regex.Replace(res, "[^a-z0-9]", "-");
			res = Regex.Replace(res, "-+", "-");
			res = res.Trim('-');

			return res;
		}

		public static void DilDegistir(string culture = "en-US")
		{
			var ci = new CultureInfo(culture);

			ci.NumberFormat.NumberDecimalSeparator = ".";
			ci.NumberFormat.NumberGroupSeparator = ",";
			ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
			ci.DateTimeFormat.LongDatePattern = "dd.MM.yyyy hh:mm:ss";
			ci.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			ci.DateTimeFormat.ShortTimePattern = "HH:mm";
			ci.DateTimeFormat.FullDateTimePattern = "dd.MM.yyyy hh:mm:ss";

			System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;
		}

		//public static int GetLanguage(string culture)
		//{
		//    var languageId = 0;

		//    using (var _context = new SenPilic_ATFContext())
		//    {
		//        var lang = _context.Languages.AsNoTracking()
		//            .FirstOrDefault(x => x.Code.ToLower() == culture.ToLower());

		//        if (lang != null)
		//            languageId = lang.Id;
		//    }

		//    return languageId;
		//}

		public static string GenerateRandomString(int length)
		{
			var random = new Random();

			const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}