using Common.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class RazorHelper
    {
        #region List Page Operations

        public static string GenerateTHead(IEnumerable<SearchHeader> items)
        {
            var sb = new StringBuilder();

            sb.Append("<tr class='search-box'>");
            foreach (var item in items)
            {
                if (item != null)
                {
                    if (item == items.First())
                    {
                        if (item.DataEnable)
                            sb.Append("<td></td>");
                        else sb.Append("<td class='input-pe-disabled disabled-control'></td>");
                    }
                    else
                    {
                        var cls = "form-control search-frm " + (item.DataEnable == false ? "input-pe-disabled disabled-control" : "");
                        if (item.DataColumnType == "int")
                            cls += " intOnly";
                        else if (item.DataColumnType == "decimal" || item.DataColumnType == "double" || item.DataColumnType == "float")
                            cls += " doubleOnly";
                        else if (item.DataColumnType == "date" || item.DataColumnType == "datetime")
                            cls += " dateOnly";
                        else if (item.DataColumnType == "time")
                            cls += " timeOnly";

                        sb.Append("<td>" +
                            "<div class='input-group'>" +
                            "<input type='" + item.DataType +
                            "' name='search-form' class='" + cls + "'" +
                            " data-isChildCollection='" + item.IsChildCollection +
                            "' data-href='" + item.DataHref +
                            "' data-type='" + item.DataColumnType +
                            "' data-column-type='" + item.DataColumnType +
                            "' data-column='" + item.DataColumn + "' />" +
                            "<span class='input-group-addon clear-search-content muted pe-disabled'><i class='fa fa-remove'></i></span></td>");
                    }
                }
            }

            sb.Append("</tr>");
            sb.Append("<tr>");

            foreach (var item in items)
            {
                if (item != null)
                {
                    if (item == items.First())
                    {
                        sb.Append("<td><input type='checkbox' class='check-all' /></td>");
                    }
                    else
                    {
                        sb.Append("<td><div class='thead-sort " + (item.DataSortEnable ? "sorted-td" : "") +
                            "' data-href='" + item.DataHref +
                            "' data-sort-column='" + item.DataColumn +
                            "' data-sort-way='" + item.DataSortWay + "'>" +
                            "<span class='pull-left'>" + item.Header + "</span>" +
                            (item.DataSortEnable ? "<span class='pull-right'><i class='sort fa fa-sort-amount-asc'></i></span>" : "") +
                            "</div></td>");
                    }
                }
            }

            sb.Append("</tr>");

            return sb.ToString();
        }

        public static string GenerateTFoot(int totalRowCount, int totalPageCount, int currentPage, string href)
        {
            var cnt = string.Empty;
            var sb = new StringBuilder();

            sb.Append("<tr><td colspan='100%'><div class='pt15 pb15'>");
            cnt = FormatNumber(totalRowCount);
            sb.Append("<div class='col-xs-12 np'><div class='pull-left'>");

            if (totalPageCount > 1)
            {
                var count = Math.Round((double)totalPageCount / (double)5);

                sb.Append("<ul class='pagination'>");

                if (count > 1)
                {
                    if (currentPage > 4)
                    {
                        var current = currentPage - (currentPage % 5);

                        if (totalPageCount == currentPage)
                        {
                            current = totalPageCount;
                        }

                        if (current + 5 > totalPageCount)
                        {
                            current = totalPageCount - 5;
                        }

                        sb.Append("<li class='page-item'><a class='page-link' href ='javascript:void(0);' data-page='" + 1 + "' data-href='" + href + "'>İlk</a></li>");

                        if (currentPage > 1)
                        {
                            sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + (currentPage - 1) + "' data-href='" + href + "'>" + "<i class='fa fa-angle-double-left'></i>" + "</a></li>");
                        }

                        for (int i = current; i <= ((current + 5 <= totalPageCount) ? (current + 5) : totalPageCount); i++)
                        {
                            if (i == currentPage)
                            {
                                sb.Append("<li class='disabled page-item'><a class='page-link' href='javascript:void(0);'><strong>" + i + "</strong></a></li>");
                            }
                            else
                            {
                                sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + i + "' data-href='" + href + "'>" + i + "</a></li>");
                            }
                        }

                        if (currentPage < totalPageCount)
                        {
                            sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + (current + 6 > totalPageCount ? totalPageCount : current + 6) + "' data-href='" + href + "'>" + "<i class='fa fa-angle-double-right'></i>" + "</a></li>");
                        }

                        if (currentPage != totalPageCount)
                        {
                            sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + totalPageCount + "' data-href='" + href + "'>Son</a></li>");
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            if (i == currentPage)
                            {
                                sb.Append("<li class='disabled page-item'><a class='page-link' href='javascript:void(0);'><strong>" + i + "</strong></a></li>");
                            }
                            else
                            {
                                sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + i + "' data-href='" + href + "'>" + i + "</a></li>");

                                if (i > 4)
                                {
                                    sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + 6 + "' data-href='" + href + "'>" + "<i class='fa fa-angle-double-right'></i>" + "</a></li>");

                                    if (totalPageCount > 5)
                                    {
                                        sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + totalPageCount + "' data-href='" + href + "'>Son</a></li>");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= totalPageCount; i++)
                    {
                        if (i == currentPage)
                        {
                            sb.Append("<li class='disabled page-item'><a class='page-link' href='javascript:void(0);'><strong>" + i + "</strong></a></li>");
                        }
                        else
                        {
                            sb.Append("<li class='page-item'><a class='page-link' href='javascript:void(0);' data-page='" + i + "' data-href='" + href + "'>" + i + "</a></li>");
                        }
                    }
                }

                sb.Append("</ul>");
            }

            sb.Append("</div></div><div class='clearfix'></div>");
            sb.Append("<div>" +
                "<div class='col-xs-12 np'>" +
                "<p><strong class='text-left'>Toplam:</strong> <span id='recordCount'>" + (cnt == "0" ? "0" : cnt) + "</span></p>" +
                "</div>");
            sb.Append("</div></td></tr>");

            return sb.ToString();
        }

        private static string FormatNumber(int num)
        {
            //if (num > 9999)
            //{
            //    return num.ToString("#,##0, BİN", CultureInfo.InvariantCulture);
            //}
            //else if (num > 999999)
            //{
            //    return num.ToString("#,##0,, MİLYON", CultureInfo.InvariantCulture);
            //}
            //else if (num > 999999999)
            //{
            //    return num.ToString("#,##0,,, MİLYAR", CultureInfo.InvariantCulture);
            //}
            //else
            //{
            //    return num.ToString("#,##");
            //}

            return num.ToString("#,###", CultureInfo.InvariantCulture);
        }

        #endregion List Page Operations

        #region Form Operations

        public static string GenerateTHeadingForm(IEnumerable<SearchHeader> items)
        {
            var sb = new StringBuilder();

            sb.Append("<tr class='search-box'>");
            sb.Append("<td></td>");
            sb.Append("<td colspan = '" + (items.Count() - 1) + "'>" +
                        "<div class='pull-left'>" +
                            "<button id='sublistbtn_edit' class='btn btn-white' type='button'><i class='fa fa-pencil'></i></button>" +
                            "&nbsp;" +
                            "<button id='sublistbtn_delete' class='btn btn-white' type='button'><i class='fa fa-trash-o'></i></button>" +
                        "</div>" +
                        "<div class='pull-right'>" +
                            "<div class='input-group'>" +
                                    "<input type='text' name='search-form' class='form-control search-frm sublistsearch' />" +
                                    "<span class='input-group-addon clear-search-content muted pe-disabled'><i class='fa fa-remove'></i></span>" +
                            "</div>" +
                        "</div>" +
                    "</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");

            foreach (var item in items)
            {
                if (item == items.First())
                {
                    sb.Append("<td><input type='checkbox' class='check-all' /></td>");
                }
                else
                {
                    sb.Append("<td><div class='thead-sort'>" + item.Header + "</div></td>");
                }
            }

            sb.Append("</tr>");

            return sb.ToString();
        }

        #endregion Form Operations

        public static string GenerateSearchQuery(IEnumerable<SearchObj> parameters, bool isView = false)
        {
            var query = new StringBuilder();

            if (!isView)
            {
                query.Append("IsDeleted == false");
            }
            else
            {
                query.Append("1 == 1");
            }

            if (parameters != null)
            {
                //foreach (var item in parameters.Where(x => x.DataColumn.Split('.').Count() <= 1))
                foreach (var item in parameters)
                {
                    if (!item.DataColumn.Contains("."))
                    {
                        if (item.DataType.ToLower() == "bool")
                        {
                            if (item.DataValue.ToLower() == "active" || item.DataValue.ToLower() == "passive")
                            {
                                query.Append(" && " + item.DataColumn + " == " + (item.DataValue.ToLower() == "active" ? true : false) + " ");
                            }
                            else if (item.DataValue.ToLower() == "yes" || item.DataValue.ToLower() == "no")
                            {
                                query.Append(" && " + item.DataColumn + " == " + (item.DataValue.ToLower() == "yes" ? true : false) + " ");
                            }
                        }
                        else if (item.DataType.ToLower() == "int" || item.DataType.ToLower() == "float" || item.DataType.ToLower() == "decimal" || item.DataType.ToLower() == "double")
                        {
                            query.Append(" && " + item.DataColumn + " == " + item.DataValue);
                        }
                        else if (item.DataType.ToLower() == "date")
                        {
                            var dateStart = new DateTime();

                            if (DateTime.TryParse(item.DataValue, out dateStart))
                            {
                                var dateEnd = dateStart.AddHours(23).AddHours(59).AddMinutes(59).AddSeconds(59);

                                query.Append(" && " + item.DataColumn + " >= DateTime(" + dateStart.Year + "," + dateStart.Month + "," + dateStart.Day + "," + dateStart.Hour + "," + dateStart.Minute + "," + dateStart.Second + ")");
                                query.Append(" && " + item.DataColumn + " <= DateTime(" + dateEnd.Year + "," + dateEnd.Month + "," + dateEnd.Day + "," + dateEnd.Hour + "," + dateEnd.Minute + "," + dateEnd.Second + ")");
                            }
                        }
                        else if (item.DataType.ToLower() == "time")
                        {
                            var dt = new DateTime();

                            DateTime.TryParse(item.DataValue, out dt);

                            var ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);

                            if (dt != new DateTime())
                            {
                                query.Append(" && " + item.DataColumn + " == TimeSpan(" + dt.Hour + "," + dt.Minute + "," + dt.Second + ")");
                            }
                        }
                        else if (item.DataType.ToLower() == "enum")
                        {
                            var enumType = item.DataColumnType;

                            EnumHelper.GetValueOf(enumType, item.DataValue);
                        }
                        else
                        {
                            if (item.DataColumn.Contains(","))
                            {
                                query.Append(" && (");

                                var columns = item.DataColumn.Split(',');

                                foreach (var col in columns)
                                {
                                    query.Append(col + ".ToLower().Contains(\"" + item.DataValue + "\")");

                                    if (col != columns.Last())
                                        query.Append(" || ");
                                }

                                query.Append(") ");
                            }
                            else
                            {
                                query.Append(" && " + item.DataColumn + ".ToLower().Contains(\"" + item.DataValue + "\")");
                            }
                        }
                    }
                    else if (!item.IsChildCollection)
                    {
                        if (item.DataType.ToLower() == "bool")
                        {
                            if (item.DataValue.ToLower() == "active" || item.DataValue.ToLower() == "passive")
                            {
                                query.Append(" && " + item.DataColumn + " == " + (item.DataValue.ToLower() == "active" ? true : false) + " ");
                            }
                            else if (item.DataValue.ToLower() == "yes" || item.DataValue.ToLower() == "no")
                            {
                                query.Append(" && " + item.DataColumn + " == " + (item.DataValue.ToLower() == "yes" ? true : false) + " ");
                            }
                        }
                        else if (item.DataType.ToLower() == "int" || item.DataType.ToLower() == "float" || item.DataType.ToLower() == "decimal" || item.DataType.ToLower() == "double")
                        {
                            query.Append(" && " + item.DataColumn + " == " + item.DataValue);
                        }
                        else if (item.DataType.ToLower() == "date")
                        {
                            var dateStart = new DateTime();
                            DateTime.TryParse(item.DataValue, out dateStart);

                            var dateEnd = dateStart.AddHours(23).AddHours(59).AddMinutes(59).AddSeconds(59);

                            if (dateStart != new DateTime())
                            {
                                query.Append(" && " + item.DataColumn + " >= DateTime(" + dateStart.Year + "," + dateStart.Month + "," + dateStart.Day + "," + dateStart.Hour + "," + dateStart.Minute + "," + dateStart.Second + ")");
                                query.Append(" && " + item.DataColumn + " <= DateTime(" + dateEnd.Year + "," + dateEnd.Month + "," + dateEnd.Day + "," + dateEnd.Hour + "," + dateEnd.Minute + "," + dateEnd.Second + ")");
                            }
                        }
                        else if (item.DataType.ToLower() == "time")
                        {
                            var dt = new DateTime();
                            DateTime.TryParse(item.DataValue, out dt);

                            var ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);

                            if (dt != new DateTime())
                            {
                                query.Append(" && " + item.DataColumn + " == TimeSpan(" + dt.Hour + "," + dt.Minute + "," + dt.Second + ")");
                            }
                        }
                        else if (item.DataType.ToLower() == "enum")
                        {
                            var enumType = item.DataColumnType;

                            EnumHelper.GetValueOf(enumType, item.DataValue);
                        }
                        else
                        {
                            if (item.DataColumn.Contains(","))
                            {
                                query.Append(" && (");

                                var columns = item.DataColumn.Split(',');

                                foreach (var col in columns)
                                {
                                    query.Append(col + ".ToLower().Contains(\"" + item.DataValue + "\")");
                                    if (col != columns.Last())
                                        query.Append(" || ");
                                }

                                query.Append(") ");
                            }
                            else
                            {
                                query.Append(" && " + item.DataColumn + ".ToLower().Contains(\"" + item.DataValue + "\")");
                            }
                        }
                    }
                    else if (item.DataColumn.Split('.').Count() <= 2)
                    {
                        var arr = item.DataColumn.Split('.');

                        if (item.DataType.ToLower() == "bool")
                        {
                            if (item.DataValue.ToLower() == "active" || item.DataValue.ToLower() == "passive")
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " == " + (item.DataValue.ToLower() == "active" ? true : false) + ") ");
                            }
                            else if (item.DataValue.ToLower() == "yes" || item.DataValue.ToLower() == "no")
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " == " + (item.DataValue.ToLower() == "yes" ? true : false) + ") ");
                            }
                        }
                        else if (item.DataType == "int" || item.DataType == "float" || item.DataType == "decimal" || item.DataType == "double")
                        {
                            query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " == " + item.DataValue + ")");
                        }
                        else if (item.DataType.ToLower() == "date")
                        {
                            var dateStart = new DateTime();
                            DateTime.TryParse(item.DataValue, out dateStart);

                            var dateEnd = dateStart.AddHours(23).AddHours(59).AddMinutes(59).AddSeconds(59);

                            if (dateStart != new DateTime())
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " >= DateTime(" + dateStart.Year + "," + dateStart.Month + "," + dateStart.Day + "," + dateStart.Hour + "," + dateStart.Minute + "," + dateStart.Second + "))");
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " => DateTime(" + dateEnd.Year + "," + dateEnd.Month + "," + dateEnd.Day + "," + dateEnd.Hour + "," + dateEnd.Minute + "," + dateEnd.Second + "))");
                            }
                        }
                        else if (item.DataType.ToLower() == "time")
                        {
                            var dt = new DateTime();
                            DateTime.TryParse(item.DataValue, out dt);

                            var ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);

                            if (dt != new DateTime())
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[0] + "." + arr[1] + " == TimeSpan(" + dt.Hour + "," + dt.Minute + "," + dt.Second + "))");
                            }
                        }
                        else if (item.DataType.ToLower() == "enum")
                        {
                            var enumType = item.DataColumnType;

                            EnumHelper.GetValueOf(enumType, item.DataValue);
                        }
                        else
                        {
                            query.Append(" && " + arr[0] + ".ANY(" + arr[1] + ".ToLower().Contains(\"" + item.DataValue + "\"))");
                        }
                    }
                    else if (item.DataColumn.Split('.').Count() == 3)
                    {
                        var arr = item.DataColumn.Split('.');

                        if (item.DataType.ToLower() == "bool")
                        {
                            if (item.DataValue.ToLower() == "active" || item.DataValue.ToLower() == "passive")
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + "." + arr[2] + " == " + (item.DataValue.ToLower() == "active" ? true : false) + ") ");
                            }
                            else if (item.DataValue.ToLower() == "yes" || item.DataValue.ToLower() == "no")
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + "." + arr[2] + " == " + (item.DataValue.ToLower() == "yes" ? true : false) + ") ");
                            }
                        }
                        else if (item.DataType.ToLower() == "int" || item.DataType.ToLower() == "float" || item.DataType.ToLower() == "decimal" || item.DataType.ToLower() == "double")
                        {
                            query.Append(" && " + arr[0] + ".ANY(" + arr[1] + "." + arr[2] + " == " + item.DataValue + ")");
                        }
                        else if (item.DataType.ToLower() == "date")
                        {
                            var dateStart = new DateTime();
                            DateTime.TryParse(item.DataValue, out dateStart);

                            var dateEnd = dateStart.AddHours(23).AddHours(59).AddMinutes(59).AddSeconds(59);

                            if (dateStart != new DateTime())
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " >= DateTime(" + dateStart.Year + "," + dateStart.Month + "," + dateStart.Day + "," + dateStart.Hour + "," + dateStart.Minute + "," + dateStart.Second + "))");
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + " => DateTime(" + dateEnd.Year + "," + dateEnd.Month + "," + dateEnd.Day + "," + dateEnd.Hour + "," + dateEnd.Minute + "," + dateEnd.Second + "))");
                            }
                        }
                        else if (item.DataType.ToLower() == "time")
                        {
                            var dt = new DateTime();
                            DateTime.TryParse(item.DataValue, out dt);

                            var ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);

                            if (dt != new DateTime())
                            {
                                query.Append(" && " + arr[0] + ".ANY(" + arr[1] + "." + arr[2] + " == TimeSpan(" + dt.Hour + "," + dt.Minute + "," + dt.Second + "))");
                            }
                        }
                        else if (item.DataType.ToLower() == "enum")
                        {
                            var enumType = item.DataColumnType;

                            EnumHelper.GetValueOf(enumType, item.DataValue);
                        }
                        else
                        {
                            query.Append(" && " + arr[0] + ".ANY(" + arr[1] + "." + arr[2] + ".ToLower().Contains(\"" + item.DataValue + "\"))");
                        }
                    }
                }
            }

            return query.ToString();
        }

        /// <summary>
        /// Liste sayfalarindaki siralama seceneklerini otomatik alip sorgusunu olusturur
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GenerateSortQuery(IEnumerable<SortObj> parameters)
        {
            var query = new StringBuilder();

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    if (item.SortColumn.Split('.').Count() <= 2)
                    {
                        query.Append(item.SortColumn + " " + item.SortWay);
                    }
                    else if (item.SortColumn.Split('.').Count() == 3)
                    {
                        var arr = item.SortColumn.Split('.');

                        query.Append(arr[0] + ".ANY(" + arr[1] + "." + arr[2] + ") " + item.SortWay);
                    }

                    if (item != parameters.Last())
                    {
                        query.Append(", ");
                    }
                }
            }
            else
            {
                query.Append("ID DESC");
            }

            return query.ToString();
        }

        public static string GenerateParamsQuery(string queryParams)
        {
            var query = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(queryParams))
            {
                var queryParamItems = queryParams.Split(',');

                foreach (var item in queryParamItems)
                {
                    query.Append(" && " + item.Replace("=", "=="));
                }
            }

            return query.ToString();
        }
    }
}
