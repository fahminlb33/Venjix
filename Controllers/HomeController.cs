using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.Database;
using Venjix.Infrastructure.Services.Options;
using Venjix.Infrastructure.Services.Telegram;
using Venjix.Models.ViewModels;

namespace Venjix.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly VenjixContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly HealthCheckService _healthCheck;
        private readonly ITelegramService _telegramService;
        private readonly IVenjixOptionsService _optionsService;

        public HomeController(ILogger<HomeController> logger, IVenjixOptionsService optionsService, IMapper mapper, ITelegramService telegramService, HealthCheckService healthCheck, VenjixContext context)
        {
            _logger = logger;
            _optionsService = optionsService;
            _mapper = mapper;
            _telegramService = telegramService;
            _healthCheck = healthCheck;
            _context = context;
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Index()
        {
            var model = new DashboardModel
            {
                SensorsCount = await _context.Sensors.CountAsync(),
                RecordedDataCount = await _context.Recordings.LongCountAsync(),
                LastUpdate = (await _context.Recordings.OrderByDescending(x=>x.RecordingId).FirstOrDefaultAsync())?.Timestamp
            };

            return View(model);
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

            model.HealthStatus = health.Status.ToString();
            model.HealthChecks = health.Entries.ToDictionary(x => x.Key, y => y.Value.Status.ToString());

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

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> KeySave(SettingsModel model)
        {
            _optionsService.Options.ApiKey = model.ApiKey;
            await _optionsService.Save();

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