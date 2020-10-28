using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.DataTables;
using Venjix.Infrastructure.Helpers;
using Venjix.Models;

namespace Venjix.Controllers
{
    public class SensorsController : Controller
    {
        private readonly VenjixContext _context;
        private readonly IDataTables _dataTables;
        private readonly IMapper _mapper;

        public SensorsController(VenjixContext context, IDataTables dataTables, IMapper mapper)
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

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Statistics()
        {
            return View("Statistics");
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Add()
        {
            return View("Edit", new SensorEditModel());
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                TempData[ViewKeys.Message] = "Sensor does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            var model = _mapper.Map<SensorEditModel>(sensor);
            model.IsEdit = true;

            return View("Edit", model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null)
            {
                TempData[ViewKeys.Message] = "Sensor does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();

            TempData[ViewKeys.Message] = "Sensor deleted successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Save(SensorEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var entity = _mapper.Map<Sensor>(model);
            if (model.IsEdit)
            {
                _context.Sensors.Update(entity);
            }
            else
            {
                _context.Sensors.Add(entity);
            }

            await _context.SaveChangesAsync();

            TempData[ViewKeys.Message] = "Sensor saved successfully.";
            TempData[ViewKeys.IsSuccess] = true;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TableData([FromBody] DataTablesRequestModel req)
        {
            return Json(await _dataTables.PopulateTable(req, _context.Sensors));
        }
    }
}
