using System;

namespace Venjix.Models.ViewModels
{
    public class DashboardModel
    {
        public int SensorsCount { get; set; }
        public long RecordedDataCount { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string VenjixVersion { get; set; } = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
    }
}
