using System;

namespace Venjix.Models.ViewModels
{
    public class SensorsStatisticsModel
    {
        public int SensorId { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastUpdated { get; set; }
        public int RecordedData { get; set; }
    }
}
