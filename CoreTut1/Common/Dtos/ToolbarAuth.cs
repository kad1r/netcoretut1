namespace Common.Dtos
{
	public class ToolbarAuth
	{
		public bool Read { get; set; } = true;
		public bool Insert { get; set; } = true;
		public bool InsertData { get; set; } = true;
		public bool Update { get; set; } = true;
		public bool Save { get; set; } = true;
		public bool Submit { get; set; } = false;
		public bool Delete { get; set; } = true;
		public bool Refresh { get; set; } = true;
		public bool Select { get; set; } = true;
		public bool SelectAndClose { get; set; } = false;
		public bool Print { get; set; } = false;
		public bool Close { get; set; } = true;
		public bool Excel { get; set; } = true;
		public bool Integration { get; set; } = false;
		public int MenuId { get; set; }
		public string HelpUrl { get; set; }
	}
}