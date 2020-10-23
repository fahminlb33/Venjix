﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Venjix.Infrastructure.Helpers
{
    [HtmlTargetElement("a", Attributes = "collapsed-when")]
    public class NavbarCollapsedHelper : TagHelper
    {
        public string CollapsedWhen { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (CollapsedWhen == null)
                return;

            var targetController = CollapsedWhen.Split("/")[1];
            var targetAction = CollapsedWhen.Split("/")[2];
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
            if (!actions.Any(x => x.Equals(currentAction)))
            {
                Debug.Print("Navbar Colapse RUN");
                if (output.Attributes.ContainsName("class"))
                {
                    var lastAttr = output.Attributes["class"].Value.ToString();
                    output.Attributes.SetAttribute("class", lastAttr.Replace("collapsed", ""));
                }

                output.Attributes.SetAttribute("aria-expanded", true);
            }
        }

    }
}
