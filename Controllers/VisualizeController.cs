using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.Database;
using Venjix.Infrastructure.Helpers;
using Venjix.Infrastructure.Services.DataTables;
using Venjix.Models.Dtos;
using Venjix.Models.ViewModels;

namespace Venjix.Controllers
{
    public class VisualizeController : Controller
    {
        private readonly VenjixContext _context;
        private readonly List<SelectListItem> _updateIntervals;

        public VisualizeController(VenjixContext context)
        {
            _context = context;
            _updateIntervals = new List<SelectListItem>
            {
                new SelectListItem("Don't auto update", "0"),
                new SelectListItem("Every 10 seconds", "10"),
                new SelectListItem("Every 30 seconds", "30"),
                new SelectListItem("Every minute", "60"),
                new SelectListItem("Every 5 minutes", "300"),
                new SelectListItem("Every 10 minutes", "600"),
                new SelectListItem("Every 15 minutes", "900"),
                new SelectListItem("Every 30 minutes", "1800"),
                new SelectListItem("Every hour", "3600")
            };
        }

        #region Scatter Route

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Scatter()
        {
            return View("Scatter", await CreateScatterModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> ScatterData([FromBody] VisualizeScatterModel model)
        {
            var aRecords = await _context.Recordings.Where(x => x.SensorId == model.SensorAId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate)
                .ToListAsync();
            var bRecords = await _context.Recordings.Where(x => x.SensorId == model.SensorBId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate)
                .ToListAsync();

            var minCount = Math.Min(aRecords.Count, bRecords.Count);
            if (aRecords.Count > minCount)
            {
                aRecords.RemoveRange(minCount, aRecords.Count - minCount);
            }
            if (bRecords.Count > minCount)
            {
                bRecords.RemoveRange(minCount, bRecords.Count - minCount);
            }

            return Json(aRecords.Zip(bRecords).Select(x => new[] { x.First.Value, x.Second.Value }));
        }

        #endregion

        #region Time Series Route

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TimeSeries()
        {
            return View("TimeSeries", await CreateFilterModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TimeSeriesData([FromBody] VisualizeFilterModel model)
        {
            var records = await _context.Recordings.Where(x => x.SensorId == model.SensorId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate)
                .ToListAsync();

            return Json(records.Select(p => new
            {
                x = p.Timestamp,
                y = p.Value
            }));
        }

        #endregion

        #region Table Routes

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Table()
        {
            return View("Table", await CreateFilterModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TableData([FromBody] VisualizeTableRequestDto model)
        {
            var chain = _context.Recordings.Where(x => x.SensorId == model.SensorId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate);

            if (model.Ordering.Count > 0)
            {
                var ordering = model.Ordering.First();
                if (ordering.Column == 0)
                {
                    chain = ordering.Direction == DataTablesOrdering.Ascending
                        ? chain.OrderBy(x => x.Timestamp)
                        : chain.OrderByDescending(x => x.Timestamp);
                }
                else
                {
                    chain = ordering.Direction == DataTablesOrdering.Ascending
                        ? chain.OrderBy(x => x.Value)
                        : chain.OrderByDescending(x => x.Value);
                }
            }

            var filterCount = await chain.CountAsync();
            var recordset = await chain.Skip(model.Start).Take(model.Length).ToListAsync();
            return Json(new DataTablesResponseModel
            {
                Draw = model.Draw + 1,
                Data = recordset,
                RecordsFiltered = filterCount,
                RecordsTotal = await _context.Recordings.CountAsync(x => x.SensorId == model.SensorId)
            });
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> TableExport([FromBody] VisualizeTableRequestDto model)
        {
            var records = await _context.Recordings.Where(x => x.SensorId == model.SensorId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate)
                .Select(x => new CsvRecordDto { Timestamp = x.Timestamp, Value = x.Value })
                .ToListAsync();

            return File(await CommonHelpers.SerializeCsvRecords(records), "text/csv", "export.csv");
        }

        #endregion

        #region Common Methods 

        private async Task<VisualizeFilterModel> CreateFilterModel()
        {
            return new VisualizeFilterModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Now,
                Sensors = await _context.Sensors.Select(x => new SelectListItem(x.DisplayName, x.SensorId.ToString())).ToListAsync(),
                UpdateIntervals = _updateIntervals
            };
        }

        private async Task<VisualizeScatterModel> CreateScatterModel()
        {
            return new VisualizeScatterModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Now,
                Sensors = await _context.Sensors.Select(x => new SelectListItem(x.DisplayName, x.SensorId.ToString())).ToListAsync()
            };
        }

        #endregion

    }
}