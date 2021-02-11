using System.ComponentModel.DataAnnotations;

namespace Venjix.Infrastructure.DAL
{
    public class Trigger
    {
        [Key]
        public int TriggerId { get; set; }

        public string Name { get; set; }
        public TriggerEvent Event { get; set; }
        public TriggerTarget Target { get; set; }
        public double Value { get; set; }
        public bool ContinuousSend { get; set; }
        public int? WebhookId { get; set; }
        public Webhook Webhook { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}