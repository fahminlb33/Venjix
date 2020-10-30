using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.Services;

namespace Venjix.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/data")]
    public class ApiDataController : ControllerBase
    {
        private readonly VenjixContext _context;
        private readonly ITriggerRunnerService _triggerRunner;

        public ApiDataController(VenjixContext context, ITriggerRunnerService triggerRunner)
        {
            _context = context;
            _triggerRunner = triggerRunner;
        }

        [HttpGet, HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveDataByQuery()
        {
            try
            {
                Dictionary<string, double> dict;

                // check form
                if (HttpContext.Request.HasFormContentType)
                {
                    var form = HttpContext.Request.Form;
                    dict = form.ToDictionary(x => x.Key, y => double.Parse(y.Value));
                }
                else
                {
                    // check query
                    if (HttpContext.Request.Query.Count > 0)
                    {
                        var queries = HttpContext.Request.Query;
                        dict = queries.ToDictionary(x => x.Key, y => double.Parse(y.Value));
                    }
                    else
                    {
                        // check body
                        using var body = HttpContext.Request.Body;
                        using var sr = new StreamReader(body);
                        using var jr = new JsonTextReader(sr);

                        var serializer = new JsonSerializer();
                        dict = serializer.Deserialize<Dictionary<string, double>>(jr);
                    }
                }

                // store to db
                var count = 0;
                var sensors = await _context.Sensors.ToListAsync();
                var recordings = new List<Recording>();
                var now = DateTime.Now;
                foreach (var entry in dict)
                {
                    var sensor = sensors.Find(x => x.ApiField == entry.Key);
                    if (sensor == null) continue;

                    var record = new Recording
                    {
                        Timestamp = now,
                        Sensor = sensor,
                        SensorId = sensor.SensorId,
                        Value = entry.Value
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