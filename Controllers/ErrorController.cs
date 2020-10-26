using Microsoft.AspNetCore.Mvc;

namespace Venjix.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnauthorizedPage()
        {
            return View();
        }
    }
}
