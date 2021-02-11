using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Venjix.Infrastructure.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "disabled-when")]
    public class DisabledAttributeHelper : TagHelper
    {
        public bool DisabledWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (DisabledWhen)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
            else
            {
                if (output.Attributes.ContainsName("disabled"))
                {
                    output.Attributes.Remove(output.Attributes.First(x => x.Name == "disabled"));
                }
            }
        }
    }
}