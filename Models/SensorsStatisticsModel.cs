using System;

namespace Venjix.Models
{
    public class SensorsStatisticsModel
    {
        public string DisplayName { get; set; }
        public DateTime LastUpdated { get; set; }
        public int RecordedData { get; set; }
    }
}
