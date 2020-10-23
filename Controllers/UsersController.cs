using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Profile()
        {
            return View("Edit");
        }
    }
}
