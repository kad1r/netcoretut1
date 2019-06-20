namespace Common.Dtos
{
	public class SearchObj
	{
		public string DataColumn { get; set; }
		public string DataType { get; set; }
		public string DataValue { get; set; }
		public string DataColumnType { get; set; }
		public string DataEnumType { get; set; }
		public bool IsChildCollection { get; set; } = true;
		public bool IsQueryString { get; set; } = false;
	}
}