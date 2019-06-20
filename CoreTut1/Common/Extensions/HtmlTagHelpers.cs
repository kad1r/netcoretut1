using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Common.Extensions
{
	[HtmlTargetElement("kadir")]
	public class HtmlTagHelpers : TagHelper
	{
		public string Name { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "CustomTagHelper";
			output.TagMode = TagMode.StartTagAndEndTag;

			var sb = new StringBuilder();
			sb.AppendFormat("<span>Hi! {0}</span>", this.Name);

			output.PreContent.SetHtmlContent(sb.ToString());
		}
	}

	[HtmlTargetElement("kadir")]
	public class TextBoxForHelper : TagHelper
	{
		public ModelExpression Source { get; set; }

		public string Name { get; set; }
		public string PlaceHolder { get; set; }
		public string ClassName { get; set; }
		public int MinLenght { get; set; }
		public int MaxLenght { get; set; }
		public bool Required { get; set; }
		public bool IsPassword { get; set; }
		public bool PasswordStrength { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "";
			output.TagMode = TagMode.StartTagAndEndTag;

			//var oMetaData = ExpressionMetadataProvider.FromLambdaExpression(expression, html.ViewData);

			var sb = new StringBuilder();

			if (context.AllAttributes["MinLength"] != null)
			{

			}

			sb.AppendFormat("<input type='text' name='{0}' id='{0}' class='{1}' placeholder='{2}' required='{3}' />", Name, ClassName, Source.Metadata.DisplayName, Source.Metadata.IsRequired);

			output.PreContent.SetHtmlContent(sb.ToString());
		}
	}
}