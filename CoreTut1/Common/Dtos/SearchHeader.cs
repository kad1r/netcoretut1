namespace Common.Dtos
{
	public class SearchHeader
	{
		public string Header { get; set; }
		public string DataHref { get; set; }
		public string DataColumn { get; set; }
		public string DataSortWay { get; set; }
		public string DataType { get; set; }
		public string DataColumnType { get; set; }
		public string DataEnumType { get; set; }
		public bool DataEnable { get; set; } = true;
		public bool DataSortEnable { get; set; }
		public bool IsChildCollection { get; set; }
	}
}