using Data.Model;
using Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public class UtilityHelper<T> where T : class
    {
        public static string GetMaxCode(string table, string column, string where = "", int increaseNumber = 0)
        {
            var code = string.Empty;

            using (var _uow = new UnitOfWork(new PhotographyContext()))
            {
                var dt = _uow.Repository<T>()
                    .DataTableQuery("SELECT MAX(ISNULL(TRY_CONVERT(BIGINT, " + column + "), 0)+1) AS '" + column +
                    "' FROM " + table + (!string.IsNullOrWhiteSpace(where) ? " WHERE " + where : ""));
                if (dt != null && dt.Rows.Count > 0)
                {
                    code = dt.Rows[0]["" + column + ""].ToString();
                    code = String.IsNullOrWhiteSpace(code) ? "1" : code;
                    if (increaseNumber > 0)
                        code = (int.Parse(code) + increaseNumber).ToString();
                    code = code.PadLeft(10, '0');
                }
                else
                {
                    code = "1";
                    code = code.PadLeft(10, '0');
                }
            }

            return code;
        }

        public static IEnumerable<T> MoveToTop(IEnumerable<T> list, Func<T, bool> func)
        {
            return list.Where(func).Concat(list.Where(item => !func(item)));
        }
    }
}
