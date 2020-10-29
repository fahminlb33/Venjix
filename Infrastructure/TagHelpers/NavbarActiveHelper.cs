using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Venjix.Infrastructure.TagHelpers
{
    [HtmlTargetElement("li", Attributes = "active-when")]
    [HtmlTargetElement("a", Attributes = "active-when")]
    public class NavbarActiveHelper : TagHelper
    {
        public string ActiveWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ActiveWhen == null)
                return;

            var targetController = ActiveWhen.Split("/")[1];
            var targetAction = ActiveWhen.Split("/")[2];
            var actions = new List<string>();
            if (targetAction.Contains("|"))
            {
                actions.AddRange(targetAction.Split('|'));
            }
            else
            {
                actions.Add(targetAction);
            }

            var currentController = ViewContextData.RouteData.Values["controller"].ToString();
            var currentAction = ViewContextData.RouteData.Values["action"].ToString();

            if (!currentController.Equals(targetController)) return;
            if (string.IsNullOrEmpty(targetAction) || actions.Any(x => x.Equals(currentAction)))
            {
                if (output.Attributes.ContainsName("class"))
                {
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                }
                else
                {
                    output.Attributes.SetAttribute("class", "active");
                }
            }
        }

    }
}
