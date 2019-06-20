namespace Common.Dtos
{
	public class ConnectionObj
	{
		public string ConnectionString { get; set; }
		public string DataSource { get; set; }
		public string Database { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public bool IntegratedSecurity { get; set; }
	}
}