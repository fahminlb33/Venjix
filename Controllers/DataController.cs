using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Venjix.Infrastructure.Database;
using Venjix.Infrastructure.Helpers;
using Venjix.Infrastructure.Services.Options;
using Venjix.Infrastructure.Services.Triggers;
using Venjix.Models.Entities;

namespace Venjix.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class DataController : ControllerBase
    {
        public const string ApiKeyField = "key";

        private readonly VenjixContext _context;
        private readonly IVenjixOptionsService _optionsService;
        private readonly ITriggerRunnerService _triggerRunner;

        public DataController(VenjixContext context, ITriggerRunnerService triggerRunner, IVenjixOptionsService optionsService)
        {
            _context = context;
            _triggerRunner = triggerRunner;
            _optionsService = optionsService;
        }

        [HttpGet][HttpPost]
        [Route("data")]
        public async Task<IActionResult> SaveData()
        {
            try
            {
                Dictionary<string, string> dict;

                // check form
                if (HttpContext.Request.HasFormContentType)
                {
                    var form = HttpContext.Request.Form;
                    dict = form.ToDictionary(x => x.Key, y => y.Value.ToString());
                }
                else
                {
                    // check query
                    if (HttpContext.Request.Query.Count > 0)
                    {
                        var queries = HttpContext.Request.Query;
                        dict = queries.ToDictionary(x => x.Key, y => y.Value.ToString());
                    }
                    else
                    {
                        // check body
                        using var body = HttpContext.Request.Body;
                        using var sr = new StreamReader(body);
                        using var jr = new JsonTextReader(sr);

                        var serializer = new JsonSerializer();
                        dict = serializer.Deserialize<Dictionary<string, string>>(jr);
                    }
                }

                // check api key
                if (!dict.TryGetValue(ApiKeyField, out string key))
                {
                    return BadRequest(new { message = "No API key is provided." });
                }
                else
                {
                    if (key != _optionsService.Options.ApiKey)
                    {
                        return Unauthorized(new { message = "The provided API key is invalid." });
                    }
                }

                // store to db
                var count = 0;
                var sensors = await _context.Sensors.ToListAsync();
                var recordings = new List<Recording>();
                var now = DateTime.Now.ToUniversalTime();
                foreach (var entry in dict)
                {
                    var sensor = sensors.Find(x => x.ApiField == entry.Key);
                    if (sensor == null) continue;
                    if (!double.TryParse(entry.Value, System.Globalization.NumberStyles.AllowDecimalPoint, CommonHelpers.USCulture, out double value)) continue;

                    var record = new Recording
                    {
                        Timestamp = now,
                        Sensor = sensor,
                        SensorId = sensor.SensorId,
                        Value = value
                    };

                    recordings.Add(record);
                    _context.Recordings.Add(record);
                    count++;
                }

                // save
                await _context.SaveChangesAsync();

                // run triggers
               _triggerRunner.RunTriggers(recordings);

                return Ok(count);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}