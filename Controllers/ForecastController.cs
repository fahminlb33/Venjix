using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
