using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Venjix.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class DataApiController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok(new List<string>() { "AA", "BB" });
        }

        [Route("table")]
        public IActionResult Table()
        {
            return Ok(new List<string>() { "AA", "BB" });
        }
    }
}
