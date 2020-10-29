using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Venjix.Infrastructure.Authentication;

namespace Venjix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
