using System;

namespace Venjix.Models
{
    public class DashboardModel
    {
        public int SensorsCount { get; set; }
        public long RecordedDataCount { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
