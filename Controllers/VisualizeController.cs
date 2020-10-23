using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Venjix.Controllers
{
    public class VisualizeController : Controller
    {
        public IActionResult Geomap()
        {
            return View();
        }

        public IActionResult Scatter()
        {
            return View();
        }

        public IActionResult Table()
        {
            return View();
        }

        public IActionResult TimeSeries()
        {
            return View();
        }
    }
}
