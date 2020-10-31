using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Venjix.Infrastructure;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.DataTables;
using Venjix.Infrastructure.TagHelpers;
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
        public async Task<IActionResult> Statistics()
        {
            var records = await _context.Recordings
                .GroupBy(x => x.SensorId).Select(x => new SensorsStatisticsModel
                {
                    SensorId = x.Key,
                    LastUpdated = x.Max(x => x.Timestamp),
                    RecordedData = x.Count()
                }).ToListAsync();

            var sensors = (await _context.Sensors.ToListAsync()).ToDictionary(x => x.SensorId, y => y.DisplayName);
            records = records.Select(x =>
            {
                x.DisplayName = sensors[x.SensorId];
                return x;
            }).ToList();

            return View("Statistics", records);
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Download(int sensorId)
        {
            var sensor = await _context.Sensors.FindAsync(sensorId);
            if (sensor == null)
            {
                TempData[ViewKeys.Message] = "Can't export, sensor does not exists.";
                TempData[ViewKeys.IsSuccess] = false;

                return RedirectToAction("Index");
            }

            var records = await _context.Recordings.Where(x => x.SensorId == sensorId)
                .Select(x => new
                {
                    Timestamp = x.Timestamp,
                    Value = x.Value
                })
                .ToListAsync();
            return File(await CommonHelpers.SerializeCsvRecords(records), "text/csv", sensor.DisplayName + ".csv");
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