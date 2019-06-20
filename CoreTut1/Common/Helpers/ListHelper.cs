using Common.Dtos;
using Data.Dtos;
using Data.Model;
using Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace Common.Helpers
{
    public static class ListHelper
    {
        public static string GenerateSearchQuery(IEnumerable<SearchObj> parameters)
        {
            var sb = new StringBuilder();

            sb.Append("1=1");

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    if (item.DataType == "bool")
                    {
                        //if (item.DataValue.ToLower() == "aktif")
                        //    query += " && " + item.DataColumn + " == " + (item.DataValue.ToLower() == "aktif" ? true : false) + " ";
                        //else if (item.DataValue.ToLower() == "evet" || item.DataValue.ToLower() == "hayır")
                        //    query += " && " + item.DataColumn + " == " + (item.DataValue.ToLower() == "evet" ? true : false) + " ";
                    }
                    else if (item.DataType == "int" || item.DataType == "float" || item.DataType == "decimal" || item.DataType == "double")
                    {
                        sb.Append(" && " + item.DataColumn + " == " + item.DataValue);
                    }
                    else if (item.DataType == "date")
                    {
                        var dt = new DateTime();
                        DateTime.TryParse(item.DataValue, out dt);

                        if (dt != new DateTime())
                        {
                            sb.Append(" && " + item.DataColumn + " == DateTime(" + dt.Year + "," + dt.Month + "," + dt.Day + "," + dt.Hour + "," + dt.Minute + "," + dt.Second + ")");
                        }
                    }
                    else if (item.DataType == "time")
                    {
                        var dt = new DateTime();
                        DateTime.TryParse(item.DataValue, out dt);

                        var ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);

                        if (dt != new DateTime())
                        {
                            sb.Append(" && " + item.DataColumn + " == TimeSpan(" + dt.Hour + "," + dt.Minute + "," + dt.Second + ")");
                        }
                    }
                    else if (item.DataType == "enum")
                    {
                        var enumType = item.DataColumnType;

                        // TODO: check for enum values
                    }
                    else
                    {
                        if (item.DataColumn.Contains(","))
                        {
                            //query += " && (";
                            //var columns = item.DataColumn.Split(',');

                            //foreach (var col in columns)
                            //{
                            //    query += col + ".ToLower().Contains(\"" + item.DataValue + "\")";
                            //    if (col != columns.Last())
                            //        query += " || ";
                            //}

                            //query += ") ";
                        }
                        else
                        {
                            sb.Append(" && " + item.DataColumn + ".ToLower().Contains(\"" + item.DataValue + "\")");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static string GenerateSortQuery(IEnumerable<SortObj> parameters)
        {
            var sb = new StringBuilder();

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    sb.Append(item.SortColumn + " " + item.SortWay);

                    if (item != parameters.Last())
                    {
                        sb.Append(", ");
                    }
                }
            }
            else
            {
                sb.Append("Id Desc");
            }

            return sb.ToString();
        }

        public static string GenerateTFoot(int totalRowCount, int totalPageCount, int currentPage, string href)
        {
            if (totalRowCount > 0)
            {
                var cnt = string.Empty;
                var sb = new StringBuilder();

                sb.Append("<tr><td colspan='100%'><div class='pt15 pb15'>");
                cnt = StringHelper.FormatNumber(totalRowCount);
                sb.Append("<div class='col-xs-12'><div class='pull-left'>");

                if (totalPageCount > 1)
                {
                    var count = Math.Round((double)totalPageCount / (double)5);

                    sb.Append("<ul class='pagination paging no-margins'>");

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

                            sb.Append("<li><a href='javascript:void(0);' data-page='" + 1 + "' data-href='" + href + "'>" + "İLK" + "</a></li>");

                            if (currentPage > 1)
                            {
                                sb.Append("<li><a href='javascript:void(0);' data-page='" + (currentPage - 1) + "' data-href='" + href + "'>" + "<i class='fa fa-angle-double-left'></i>" + "</a></li>");
                            }

                            for (int i = current; i <= ((current + 5 <= totalPageCount) ? (current + 5) : totalPageCount); i++)
                            {
                                if (i == currentPage)
                                {
                                    sb.Append("<li class='disabled'><a href='javascript:void(0);'><strong>" + i + "</strong></a></li>");
                                }
                                else
                                {
                                    sb.Append("<li><a href='javascript:void(0);' data-page='" + i + "' data-href='" + href + "'>" + i + "</a></li>");
                                }
                            }

                            if (currentPage < totalPageCount)
                            {
                                sb.Append("<li><a href='javascript:void(0);' data-page='" + (current + 6 > totalPageCount ? totalPageCount : current + 6) + "' data-href='" + href + "'>" + "<i class='fa fa-angle-double-right'></i>" + "</a></li>");
                            }

                            if (currentPage != totalPageCount)
                            {
                                sb.Append("<li><a href='javascript:void(0);' data-page='" + totalPageCount + "' data-href='" + href + "'>" + "SON" + "</a></li>");
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= 5; i++)
                            {
                                if (i == currentPage)
                                {
                                    sb.Append("<li class='disabled'><a href='javascript:void(0);'><strong>" + i + "</strong></a></li>");
                                }
                                else
                                {
                                    sb.Append("<li><a href='javascript:void(0);' data-page='" + i + "' data-href='" + href + "'>" + i + "</a></li>");

                                    if (i > 4)
                                    {
                                        sb.Append("<li><a href='javascript:void(0);' data-page='" + 6 + "' data-href='" + href + "'>" + "<i class='fa fa-angle-double-right'></i>" + "</a></li>");

                                        if (totalPageCount > 5)
                                        {
                                            sb.Append("<li><a href='javascript:void(0);' data-page='" + totalPageCount + "' data-href='" + href + "'>" + "SON" + "</a></li>");
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
                                sb.Append("<li class='disabled'><a href='javascript:void(0);'><strong>" + i + "</strong></a></li>");
                            }
                            else
                            {
                                sb.Append("<li><a href='javascript:void(0);' data-page='" + i + "' data-href='" + href + "'>" + i + "</a></li>");
                            }
                        }
                    }

                    sb.Append("</ul>");
                }

                sb.Append("</div></div><div class='clearfix'></div>");
                sb.Append("<div><div class='col-xs-12'><p><strong class='text-left'>" + "TOTAL: " + "</strong> <span id='recordCount'>" + cnt + "</span></p></div>");
                sb.Append("</div></td></tr>");

                return sb.ToString();
            }

            return string.Empty;
        }

        public static dynamic Paging<T>(dynamic vm, int pageSize, int page, string searchQuery, Expression<Func<T, bool>> expression) where T : class
        {
            var RowCount = 0;

            using (var _uow = new UnitOfWork(new PhotographyContext()))
            {
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    if (expression != null)
                    {
                        RowCount = _uow.Repository<T>()
                            .Query(expression, new EfConfig())
                            .Where(searchQuery)
                            .Count();
                    }
                    else
                    {
                        RowCount = _uow.Repository<T>()
                            .Query(new EfConfig())
                            .Where(searchQuery)
                            .Count();
                    }
                }
                else
                {
                    if (expression != null)
                    {
                        RowCount = _uow.Repository<T>()
                            .Query(expression, new EfConfig())
                            .Count();
                    }
                    else
                    {
                        RowCount = _uow.Repository<T>()
                            .Query(new EfConfig())
                            .Count();
                    }
                }
            }

            var TotalRowCount = Math.Ceiling(Convert.ToDecimal(RowCount) / Convert.ToDecimal(pageSize));
            vm.RowCount = RowCount;
            vm.PageCount = Convert.ToInt32(TotalRowCount);
            vm.Page = page;

            return vm;
        }
    }
}
