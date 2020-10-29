using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Venjix.Infrastructure.TagHelpers
{
    [HtmlTargetElement("*", Attributes = "readonly-when")]
    public class ReadOnlyAttributeHelper : TagHelper
    {
        public bool ReadonlyWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ReadonlyWhen)
            {
                output.Attributes.SetAttribute("readonly", "readonly");
            }
            else
            {
                if (output.Attributes.ContainsName("readonly"))
                {
                    output.Attributes.Remove(output.Attributes.First(x => x.Name == "readonly"));
                }
            }
        }
    }
}