using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.Database;
using Venjix.Infrastructure.Services.DataTables;
using Venjix.Infrastructure.TagHelpers;
using Venjix.Models.Entities;
using Venjix.Models.ViewModels;

namespace Venjix.Controllers
{
    public class TriggersController : Controller
    {
        private readonly VenjixContext _context;
        private readonly IDataTables _dataTables;
        private readonly IMapper _mapper;

        public TriggersController(VenjixContext context, IDataTables dataTables, IMapper mapper)
        {
            _context = context;
            _dataTables = dataTables;
            _mapper = mapper;
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Add()
        {
            return View("Edit", await CreateModel(default(Trigger)));
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var trigger = await _context.Triggers.FindAsync(id);
            if (trigger == null)
            {
                TempData[ViewKeys.Message] = "Trigger does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            var model = await CreateModel(trigger);
            model.IsEdit = true;

            return View("Edit", model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var trigger = await _context.Triggers.FindAsync(id);
            if (trigger == null)
            {
                TempData[ViewKeys.Message] = "Trigger does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            _context.Triggers.Remove(trigger);
            await _context.SaveChangesAsync();

            TempData[ViewKeys.Message] = "Trigger deleted successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Save(TriggerEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", await CreateModel(model));
            }

            var entity = _mapper.Map<Trigger>(model);
            if (model.IsEdit)
            {
                _context.Triggers.Update(entity);
            }
            else
            {
                _context.Triggers.Add(entity);
            }

            await _context.SaveChangesAsync();

            TempData[ViewKeys.Message] = "Trigger saved successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TableData([FromBody] DataTablesRequestModel req)
        {
            return Json(await _dataTables.PopulateTable(req, _context.Triggers));
        }

        private async Task<TriggerEditModel> CreateModel(TriggerEditModel trigger)
        {
            var model = await CreateModel(default(Trigger));
            trigger.Events = model.Events;
            trigger.Targets = model.Targets;
            trigger.Sensors = model.Sensors;
            trigger.Webhooks = model.Webhooks;

            return trigger;
        }

        private async Task<TriggerEditModel> CreateModel(Trigger trigger)
        {
            var sensors = await _context.Sensors.ToListAsync();
            var webhooks = await _context.Webhooks.ToListAsync();
            var model = new TriggerEditModel
            {
                Events = new List<SelectListItem>
                {
                    new SelectListItem("New data", ((int)TriggerEvent.NewData).ToString()),
                    new SelectListItem("New data smaller than", ((int)TriggerEvent.SmallerThan).ToString()),
                    new SelectListItem("New data smaller or equal to", ((int)TriggerEvent.SmallerOrEqual).ToString()),
                    new SelectListItem("New data larger or equal to", ((int)TriggerEvent.LargerOrEqual).ToString()),
                    new SelectListItem("New data larger than", ((int)TriggerEvent.LargerThan).ToString())
                },
                Targets = new List<SelectListItem>
                {
                    new SelectListItem("Webhook", ((int)TriggerTarget.Webhook).ToString()),
                    new SelectListItem("Telegram", ((int)TriggerTarget.Telegram).ToString())
                },
                Sensors = sensors.Select(s => new SelectListItem(s.DisplayName, s.SensorId.ToString())).ToList(),
                Webhooks = webhooks.Select(w => new SelectListItem(w.Name, w.WebhookId.ToString())).ToList()
            };

            if (trigger != null)
            {
                model = _mapper.Map(trigger, model);
            }

            return model;
        }
    }
}