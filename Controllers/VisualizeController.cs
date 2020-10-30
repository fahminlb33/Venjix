using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.DataTables;
using Venjix.Infrastructure.DTO;
using Venjix.Models;

namespace Venjix.Controllers
{
    public class VisualizeController : Controller
    {
        private readonly VenjixContext _context;

        public VisualizeController(VenjixContext context)
        {
            _context = context;
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Geomap()
        {
            return View();
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult Scatter()
        {
            return View();
        }

        [Authorize(Roles = Roles.AdminOrUser)]
        public async Task<IActionResult> Table()
        {
            var model = new VisualizeTableModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Now,
                Sensors = await _context.Sensors.Select(x => new SelectListItem(x.DisplayName, x.SensorId.ToString())).ToListAsync()
            };

            return View("Table", model);
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

        [Authorize(Roles = Roles.AdminOrUser)]
        public IActionResult TimeSeries()
        {
            return View();
        }


    }
}