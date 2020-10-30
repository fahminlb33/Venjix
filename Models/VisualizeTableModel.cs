using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Venjix.Models
{
    public class VisualizeTableModel
    {
        public int SensorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<SelectListItem> Sensors { get; set; }
    }
}