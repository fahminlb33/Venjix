using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Venjix.Controllers
{
    public class SensorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View("Edit");
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Save()
        {
            return View("Index");
        }
    }
}
