using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.DTO;
using Venjix.Infrastructure.Helpers;
using Venjix.Infrastructure.Options;
using Venjix.Models;

namespace Venjix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWritableOptions<VenjixOptions> _optionsWritable;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IWritableOptions<VenjixOptions> optionsWritable, IMapper mapper)
        {
            _logger = logger;
            _optionsWritable = optionsWritable;
            _mapper = mapper;
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult About()
        {
            return View("About");
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Settings()
        {
            return View("Settings", _mapper.Map<SettingsEditModel>(_optionsWritable.Value));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Save(SettingsEditModel model)
        {
            await _optionsWritable.Update(options => _mapper.Map(model, options));
            TempData[ViewKeys.Message] = "Settings updated successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Settings");
        }
    }
}
