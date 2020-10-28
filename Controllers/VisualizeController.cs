using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Venjix.Infrastructure.DAL;

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
            var ll = new List<SelectListItem>();
            ll.Add(new SelectListItem { Text = "AAA", Value = "ass" });
            ViewData["Items"] = ll;
            return View();
        }

        public IActionResult TimeSeries()
        {
            return View();
        }
    }
}
