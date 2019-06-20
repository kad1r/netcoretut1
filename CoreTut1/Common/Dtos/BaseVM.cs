using System.Collections.Generic;

namespace Common.Dtos
{
	public class BaseVM
	{
		public int RowCount { get; set; }
		public int PageCount { get; set; }
		public int DataCount { get; set; }
		public int Page { get; set; }
		public string Order { get; set; }
		public string OrderWay { get; set; }
		public string Keyword { get; set; }
		public List<SearchObj> Search { get; set; }
		public List<SortObj> Sort { get; set; }
		public int? ReferenceId { get; set; }
		public int? LanguageId { get; set; }
		public ToolbarAuth ToolBar { get; set; }
		public Dictionary<string, bool> RequiredFields { get; set; }
	}
}