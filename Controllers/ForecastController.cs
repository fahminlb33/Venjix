using Microsoft.AspNetCore.Mvc;

namespace Venjix.Controllers
{
    [Route("forecast")]
    public class ForecastController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}