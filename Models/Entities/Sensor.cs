using System.ComponentModel.DataAnnotations;

namespace Venjix.Models.Entities
{
    public class Sensor
    {
        [Key]
        public int SensorId { get; set; }

        public string ApiField { get; set; }
        public string DisplayName { get; set; }
    }
}