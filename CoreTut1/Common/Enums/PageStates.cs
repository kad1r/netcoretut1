namespace Common.Enums
{
	public enum PageStatesEnum
	{
		//[Display(Description = "MSG_DUZELT_OK", ResourceType = typeof(Resources.Resources))]
		Updated = 1,

		//[Display(Description = "MSG_DUZELT_HATA", ResourceType = typeof(Resources.Resources))]
		NotUpdated = 2,

		//[Display(Description = "MSG_KAYDET_OK", ResourceType = typeof(Resources.Resources))]
		Created = 3,

		//[Display(Description = "MSG_KAYDET_HATA", ResourceType = typeof(Resources.Resources))]
		NotCreated = 4,

		//[Display(Description = "MSG_SIL_OK", ResourceType = typeof(Resources.Resources))]
		Deleted = 5,

		//[Display(Description = "MSG_SIL_HATA", ResourceType = typeof(Resources.Resources))]
		NotDeleted = 6,

		//[Display(Description = "MSG_KAYIT_UYARI", ResourceType = typeof(Resources.Resources))]
		NotCompare = 7
	}
}