using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venjix.Infrastructure.Authentication;

namespace Venjix.Controllers
{
    public class SysmanController : Controller
    {
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Logs()
        {
            return View();
        }
    }
}
