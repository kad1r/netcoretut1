namespace Common.Enums
{
	public enum LogTarget
	{
		Database = 1,
		File = 2,
		EventLog = 3,
		Xml = 4,
		Exception = 5,
	}

	public enum LogType
	{
		Exception = 1,
		Info = 2,
		Warning = 3,
		Insert = 4,
		Update = 5,
		Delete = 6
	}
}