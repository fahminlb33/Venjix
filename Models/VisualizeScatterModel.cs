using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Venjix.Models
{
    public class VisualizeScatterModel
    {
        public int SensorAId { get; set; }
        public int SensorBId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<SelectListItem> Sensors { get; set; }
    }
}
