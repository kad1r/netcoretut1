using Common.Dtos;
using Common.Enums;
using System;
using System.IO;
using System.Text;
using AppContext = Common.Base.AppContext;

namespace Common.Helpers
{
	public static class LogHelper
	{
		public static string path = @"/Logs/";

		public static void WriteLog(LogTarget target, LogObj log)
		{
			try
			{
				var filename = string.Empty;
				var filePath = string.Empty;
				var sb = new StringBuilder();

				switch (target)
				{
					case LogTarget.Database:
						break;

					case LogTarget.File:
						{
							filename = "logfile_" + DateTime.Now.ToString("dd.MM.yyyy HH:mm").Replace(".", "") + ".xml";
							filePath = path + filename;
							var fi = new FileInfo(AppContext.env.ContentRootPath + filePath);

							if (!fi.Exists)
							{
								fi.Create().Close();
							}

							if (log != null)
							{
								sb.AppendLine("<log>" + Environment.NewLine);
								sb.AppendLine("<logType>" + "FILE" + "</logType>" + Environment.NewLine);
								sb.AppendLine("<date>" + log.DateInserted.ToString("dd.MM.yyyy HH:mm:ss") + "</date>" + Environment.NewLine);
								sb.AppendLine("<message>" + log.Message + "</message>" + Environment.NewLine);
								sb.AppendLine("<exceptionSource>" + log.InnerExceptionSource + "</exceptionSource>" + Environment.NewLine);
								sb.AppendLine("<exceptionMessage>" + log.InnerExceptionMessage + "</exceptionMessage>" + Environment.NewLine);
								sb.AppendLine("<exceptionStackTrace>" + log.InnerExceptionStackTrace + "</exceptionStackTrace>" + Environment.NewLine);
								if (log.arrObject.Count > 0)
								{
									sb.AppendLine("<parameters>" + Environment.NewLine);
									foreach (var item in log.arrObject)
									{
										sb.AppendLine("<" + item.Key + ">" + item.Value + "</" + item.Key + ">" + Environment.NewLine);
									}
									sb.AppendLine("</parameters>" + Environment.NewLine);
								}
								sb.AppendLine("</log>" + Environment.NewLine);
								sb.AppendLine("------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
							}

							break;
						}

					case LogTarget.EventLog:
						{
							filename = "file_" + DateTime.Now.ToString("dd.MM.yyyy").Replace(".", "") + ".xml";
							filePath = path + filename;
							var fi = new FileInfo(AppContext.env.ContentRootPath + filePath);

							if (!fi.Exists)
							{
								fi.Create().Close();
							}

							if (log != null)
							{
								sb.AppendLine("<log>" + Environment.NewLine);
								sb.AppendLine("<logType>" + "EVENT LOG" + "</logType>" + Environment.NewLine);
								sb.AppendLine("<date>" + log.DateInserted.ToString("dd.MM.yyyy HH:mm:ss") + "</date>" + Environment.NewLine);
								sb.AppendLine("<message>" + log.Message + "</message>" + Environment.NewLine);
								sb.AppendLine("</log>" + Environment.NewLine);
								sb.AppendLine("------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
							}

							break;
						}

					case LogTarget.Xml:
						break;

					case LogTarget.Exception:
						{
							filename = "exception_" + DateTime.Now.ToString("dd.MM.yyyy").Replace(".", "") + ".xml";
							filePath = path + filename;
							var fi = new FileInfo(AppContext.env.ContentRootPath + filePath);

							if (!fi.Exists)
							{
								fi.Create().Close();
							}

							if (log != null)
							{
								sb.AppendLine("<log>" + Environment.NewLine);
								sb.AppendLine("<logType>" + "EXCEPTION" + "</logType>" + Environment.NewLine);
								sb.AppendLine("<date>" + log.DateInserted.ToString("dd.MM.yyyy HH:mm:ss") + "</date>" + Environment.NewLine);
								sb.AppendLine("<controller>" + log.Controller + "</controller>" + Environment.NewLine);
								sb.AppendLine("<action>" + log.Action + "</action>" + Environment.NewLine);
								sb.AppendLine("<routedataid>" + log.RouteDataId + "</routedataid>" + Environment.NewLine);
								sb.AppendLine("<user>" + log.User + "</user>" + Environment.NewLine);
								sb.AppendLine("<message>" + log.Message + "</message>" + Environment.NewLine);
								sb.AppendLine("<exceptionSource>" + log.InnerExceptionSource + "</exceptionSource>" + Environment.NewLine);
								sb.AppendLine("<exceptionMessage>" + log.InnerExceptionMessage + "</exceptionMessage>" + Environment.NewLine);
								sb.AppendLine("<exceptionInnerMessage>" + log.InnerExceptionMessage + "</exceptionInnerMessage>" + Environment.NewLine);
								sb.AppendLine("<exceptionStackTrace>" + log.InnerExceptionStackTrace + "</exceptionStackTrace>" + Environment.NewLine);
								if (log.arrObject != null)
								{
									sb.AppendLine("<parameters>" + Environment.NewLine);
									foreach (var item in log.arrObject)
									{
										sb.AppendLine("<" + item.Key + ">" + item.Value + "</" + item.Key + ">" + Environment.NewLine);
									}
									sb.AppendLine("</parameters>" + Environment.NewLine);
								}
								sb.AppendLine("</log>" + Environment.NewLine);
								sb.AppendLine("------------------------------------------------------------------------" + Environment.NewLine);
							}

							break;
						}

					default:
						break;
				}

				using (var stw = new StreamWriter(AppContext.env.ContentRootPath + filePath, true))
				{
					stw.WriteLine(sb.ToString());
					stw.Close();
				}
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
			}
		}
	}
}