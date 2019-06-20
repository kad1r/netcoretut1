using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dtos
{
    public class Paging
    {
        public int Page { get; set; } = 1;
        public List<SearchObj> SearchParams { get; set; } = null;
        public List<SortObj> SortParams { get; set; } = null;
    }
}
