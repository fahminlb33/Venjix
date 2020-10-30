using System.Collections.Generic;

namespace Venjix.Models
{
    public class SettingsModel
    {
        public string TelegramToken { get; set; }
        public bool IsTelegramTokenValid { get; set; }
        public Dictionary<string, string> HealthChecks { get; set; }
        public string HealthStatus { get; set; }
    }
}