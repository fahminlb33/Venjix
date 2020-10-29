using System.ComponentModel.DataAnnotations;

namespace Venjix.Models
{
    public class SensorEditModel
    {
        public int SensorId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string ApiField { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string DisplayName { get; set; }

        public bool IsEdit { get; set; }
    }
}