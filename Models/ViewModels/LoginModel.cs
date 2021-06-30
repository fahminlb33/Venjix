using Venjix.Infrastructure.DataAnnotations;

namespace Venjix.Models.ViewModels
{
    public class LoginModel
    {
        [NotNullOrWhiteSpace]
        public string Username { get; set; }

        [NotNullOrWhiteSpace]
        public string Password { get; set; }
    }
}