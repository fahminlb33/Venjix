using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Venjix.ViewComponents
{
   

    [ViewComponent(Name = "HeaderPanel")]
    public class HeaderPanelViewComponent : ViewComponent
    {
        public class Model {
            public string PageSubheading { get; set; }

            public string PageHeading { get; set; } = "Heading";

            public string Action { get; set; }

            public string ActionLabel { get; set; }

            public string Controller { get; set; }
            public string ActionIcon { get; set; }
        }

        
         

        public IViewComponentResult Invoke(string pageHeading , string pageSubheading, string action = "", string actionLabel = "", string controller = "", string actionIcon = "")
        {
            Model m = new() { PageHeading = pageHeading, PageSubheading = pageSubheading, Action = action, ActionLabel = actionLabel, Controller = controller, ActionIcon = actionIcon };

            return View("Default", m);
        }

    }
}
