using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.DataTables;
using Venjix.Infrastructure.TagHelpers;
using Venjix.Models;

namespace Venjix.Controllers
{
    public class WebhooksController : Controller
    {
        private readonly VenjixContext _context;
        private readonly IDataTables _dataTables;
        private readonly IMapper _mapper;

        public WebhooksController(VenjixContext context, IDataTables dataTables, IMapper mapper)
        {
            _context = context;
            _dataTables = dataTables;
            _mapper = mapper;
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Index()
        {
            return View("Index");
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Add()
        {
            return View("Edit", new WebhookEditModel());
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var webhook = await _context.Webhooks.FindAsync(id);
            if (webhook == null)
            {
                TempData[ViewKeys.Message] = "Webhook does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            var model = _mapper.Map<WebhookEditModel>(webhook);
            model.IsEdit = true;

            return View("Edit", model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var webhook = await _context.Webhooks.FindAsync(id);
            if (webhook == null)
            {
                TempData[ViewKeys.Message] = "Webhook does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            _context.Webhooks.Remove(webhook);
            await _context.SaveChangesAsync();

            TempData[ViewKeys.Message] = "Webhook deleted successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Save(WebhookEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var entity = _mapper.Map<Webhook>(model);
            if (model.IsEdit)
            {
                _context.Webhooks.Update(entity);
            }
            else
            {
                _context.Webhooks.Add(entity);
            }

            await _context.SaveChangesAsync();

            TempData[ViewKeys.Message] = "Webhook saved successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TableData([FromBody]  DataTablesRequestModel req)
        {
            return Json(await _dataTables.PopulateTable(req, _context.Webhooks));
        }
    }
}