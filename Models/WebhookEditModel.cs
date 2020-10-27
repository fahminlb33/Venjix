using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Venjix.Models
{
    public class WebhookEditModel
    {
        public int WebhookId { get; set; }
        public string Name { get; set; }
        public string UriFormat { get; set; }
        public string BodyFormat { get; set; }
        public bool JsonBody { get; set; }
        public string Method { get; set; }
        public bool IsEdit { get; set; }

        public readonly List<SelectListItem> Methods = new List<SelectListItem>
        {
            new SelectListItem("GET", "GET"),
            new SelectListItem("POST", "POST"),
            new SelectListItem("PUT", "PUT"),
            new SelectListItem("DELETE", "DELETE"),
        };
    }
}
