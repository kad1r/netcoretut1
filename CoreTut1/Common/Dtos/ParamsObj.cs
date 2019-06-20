using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Common.Dtos
{
	public class ParamsObj
	{
		public int SearchParamId { get; set; }
		public string ElementId { get; set; }
		public int SearchParamTypeId { get; set; }
		public string DefaultValue { get; set; }
		public IEnumerable<SelectListItem> ElementData { get; set; }
	}

	public class Dropdownobj
	{
		public int Id { get; set; }
		public string Heading { get; set; }
	}
}