using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Venjix.Models.ViewModels
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
