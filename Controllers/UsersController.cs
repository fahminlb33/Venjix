using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Venjix.DAL;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.DataTables;
using Venjix.Infrastructure.Helpers;
using Venjix.Models;

namespace Venjix.Controllers
{
    public class UsersController : Controller
    {
        private readonly List<SelectListItem> _rolesSelectItems;

        private readonly VenjixContext _context;
        private readonly IDataTables _dataTables;
        private readonly IMapper _mapper;

        public UsersController(VenjixContext context, IDataTables dataTables, IMapper mapper)
        {
            _context = context;
            _dataTables = dataTables;
            _mapper = mapper;

            _rolesSelectItems = Roles.AllRoles.Select(x => new SelectListItem(x, x)).ToList();
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Index()
        {
            return View("Index");
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Add()
        {
            var model = new UserEditModel
            {
                IsEdit = false,
                Roles = _rolesSelectItems
            };

            return View("Edit", model);
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Profile()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Users.FindAsync(id);

            var model = _mapper.Map<UserEditModel>(user);
            model.IsEdit = true;
            model.Roles = _rolesSelectItems;

            return View("Edit", model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                TempData[ViewDataKeys.Message] = "User does not exists.";
                TempData[ViewDataKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            var model = _mapper.Map<UserEditModel>(user);
            model.IsEdit = true;
            model.Roles = _rolesSelectItems;

            return View("Edit", model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                TempData[ViewDataKeys.Message] = "User does not exists.";
                TempData[ViewDataKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData[ViewDataKeys.Message] = "User deleted successfully.";
            TempData[ViewDataKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Save(UserEditModel model)
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            var username = User.FindFirst(ClaimTypes.Name).Value;
            if (role != Roles.Admin && username != model.Username)
            {
                return RedirectToAction("UnauthorizedPage", "Error");
            }

            var entity = _mapper.Map<User>(model);
            if (model.IsEdit)
            {
                _context.Users.Update(entity);
            }
            else
            {
                _context.Users.Add(entity);
            }

            await _context.SaveChangesAsync();

            TempData[ViewDataKeys.IsSuccess] = true;
            if (role == Roles.User)
            {
                TempData[ViewDataKeys.Message] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }
            else
            {
                TempData[ViewDataKeys.Message] = "User saved successfully.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> TableData([FromBody] DataTablesRequestModel req)
        {
            return Json(await _dataTables.PopulateTable(req, _context.Users, x =>
            {
                x.Password = "";
                return x;
            }));
        }
    }
}
