using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.Services;
using Venjix.Models;

namespace Venjix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVenjixOptionsService _optionsService;
        private readonly IMapper _mapper;
        private readonly ITelegramService _telegramService;
        private readonly HealthCheckService _healthCheck;

        public HomeController(ILogger<HomeController> logger, IVenjixOptionsService optionsService, IMapper mapper, ITelegramService telegramService, HealthCheckService healthCheck)
        {
            _logger = logger;
            _optionsService = optionsService;
            _mapper = mapper;
            _telegramService = telegramService;
            _healthCheck = healthCheck;
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
        public async Task<IActionResult> Settings()
        {
            var health = await _healthCheck.CheckHealthAsync();
            var model = _mapper.Map<SettingsModel>(_optionsService.Options);
            model.HealthChecks = health.Entries.ToDictionary(x => x.Key, y => y.Value.Status.ToString());
            model.HealthStatus = health.Status.ToString();

            return View("Settings", model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> TelegramSave(SettingsModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.TelegramToken))
            {
                await _telegramService.VerifyAndSaveBot(model.TelegramToken);
            }
            else
            {
                _optionsService.Options.IsTelegramTokenValid = false;
                _optionsService.Options.TelegramChatId = 0;
                _optionsService.Options.TelegramToken = "";
                await _optionsService.Save();
            }
            
            return RedirectToAction("Settings");
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> TelegramRefresh()
        {
            await _telegramService.VerifyAndSaveBot(_optionsService.Options.TelegramToken);
            return RedirectToAction("Settings");
        }
    }
}