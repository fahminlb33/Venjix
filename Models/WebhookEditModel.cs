using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Venjix.Infrastructure.TagHelpers;

namespace Venjix.Models
{
    public class WebhookEditModel
    {
        public int WebhookId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [RegularExpression(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//={}]*)")]
        public string UriFormat { get; set; }

        [RequiredIf(nameof(JsonBody), true)]
        public string BodyFormat { get; set; }

        public bool JsonBody { get; set; }

        [NotNullOrWhiteSpace]
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