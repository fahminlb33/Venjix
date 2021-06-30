using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Venjix.Infrastructure.Authentication;
using Venjix.Infrastructure.Database;
using Venjix.Infrastructure.Services.Forecasting;
using Venjix.Models.ViewModels;

namespace Venjix.Controllers
{
    [Authorize(Roles = Roles.AdminOrUser)]
    public class ForecastController : Controller
    {
        private readonly IMapper _mapper;
        private readonly VenjixContext _context;
        private readonly IForecastingService _forecastingService;

        public ForecastController(VenjixContext context, IForecastingService forecastingService, IMapper mapper)
        {
            _context = context;
            _forecastingService = forecastingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new ForecastModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Now,
                Sensors = await _context.Sensors.Select(x => new SelectListItem(x.DisplayName, x.SensorId.ToString())).ToListAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> ForecastData([FromBody] ForecastModel model)
        {
            var options = _mapper.Map<ForecastingOptions>(model);
            var minimumCount = (options.SeriesLength / options.WindowSize) * 2;

            // calculate minimum data required to run forecasting
            var recordsCount = await _context.Recordings.Where(x => x.SensorId == model.SensorId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate)
                .LongCountAsync();
            if (recordsCount < minimumCount)
            {
                return Conflict(new { message = $"Not enough data to run forecasting with provided parameters. Filtered data count: {recordsCount}." });
            }

            // calculate minimum data required to run forecasting after aggregation
            var records = await _context.Recordings.Where(x => x.SensorId == model.SensorId)
                .Where(x => x.Timestamp >= model.StartDate && x.Timestamp <= model.EndDate)
                .ToListAsync();
            var data = records.GroupBy(x => new DateTime(x.Timestamp.Date.Year, x.Timestamp.Date.Month, x.Timestamp.Date.Day))
                .Select(x => new ForecastModelInput
                {
                    RecordTime = x.Key,
                    Value = (float) x.Average(x => x.Value)
                }).ToList();
            if (data.Count < minimumCount)
            {
                return Conflict(new { message = $"Not enough data to run forecasting with provided parameters after aggregation. Aggregated data count: {data.Count}." });
            }

            // run predictions
            var result = _forecastingService.RunPredictions(data, options);
            var predictedCount = result.ForecastedValues.Length;

            return Json(new
            {
                intervals = Enumerable.Range(1, predictedCount * 3),
                forecasted = data.Skip(data.Count - 2 * predictedCount).Select(x => x.Value).Concat(result.ForecastedValues),
                upperBound = data.Skip(data.Count - 2 * predictedCount).Select(x => x.Value).Concat(result.UpperBounds),
                lowerBound = data.Skip(data.Count - 2 * predictedCount).Select(x => x.Value).Concat(result.LowerBounds),
                mae = result.MAE,
                rmse = result.RMSE
            });
        }

    }
}