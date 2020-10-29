using Venjix.Infrastructure.TagHelpers;

namespace Venjix.Models
{
    public class LoginModel
    {
        [NotNullOrWhiteSpace]
        public string Username { get; set; }

        [NotNullOrWhiteSpace]
        public string Password { get; set; }
    }
}