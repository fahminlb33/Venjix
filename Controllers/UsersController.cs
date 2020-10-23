using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venjix.DAL;
using Venjix.Infrastructure.Authentication;

namespace Venjix.Controllers
{
    public class UsersController : Controller
    {

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
     
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Add()
        {
          
            return View("Edit");
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
