using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Venjix.Infrastructure.DAL;

namespace Venjix.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/data")]
    public class ApiDataController : ControllerBase
    {
        private readonly VenjixContext _context;

        public ApiDataController(VenjixContext context)
        {
            _context = context;
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
                        using var ms = new MemoryStream();
                        await body.CopyToAsync(ms);

                        ms.Seek(0, SeekOrigin.Begin);
                        dict = await JsonSerializer.DeserializeAsync<Dictionary<string, double>>(ms);
                    }
                }

                // store to db
                var count = 0;
                var sensors = await _context.Sensors.ToListAsync();
                foreach (var entry in dict)
                {
                    var sensor = sensors.Find(x => x.ApiField == entry.Key);
                    if (sensor == null) continue;

                    var record = new Recording
                    {
                        Timestamp = DateTime.Now,
                        Sensor = sensor,
                        SensorId = sensor.SensorId,
                        Value = entry.Value
                    };

                    _context.Recordings.Add(record);
                    count++;
                }

                await _context.SaveChangesAsync();
                return Ok(count);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
