using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Venjix.Infrastructure.TagHelpers;

namespace Venjix.Models
{
    public class UserEditModel
    {
        public int UserId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; }

        [NotNullOrWhiteSpace]
        public string Role { get; set; }

        public bool IsEdit { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}