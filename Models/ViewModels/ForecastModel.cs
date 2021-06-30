using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Venjix.Models.ViewModels
{
    public class ForecastModel
    {
        public int SensorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SeriesLength { get; set; } = 30;
        public int WindowSize { get; set; } = 7;
        public int Horizon { get; set; } = 7;
        public int TestSize { get; set; } = 30;
        public int ConfidenceLevel { get; set; } = 95;

        public List<SelectListItem> Sensors { get; set; }
        public List<SelectListItem> UpdateIntervals { get; set; }
    }
}
