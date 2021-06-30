using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Venjix.Models.ViewModels
{
    public class VisualizeFilterModel
    {
        public int SensorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UpdateInterval { get; set; }

        public List<SelectListItem> Sensors { get; set; }
        public List<SelectListItem> UpdateIntervals { get; set; }
    }
}