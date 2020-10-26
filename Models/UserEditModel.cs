using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Venjix.Models
{
    public class UserEditModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsEdit { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}
