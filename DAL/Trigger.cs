using System.ComponentModel.DataAnnotations;

namespace Venjix.DAL
{
    public class Trigger
    {
        [Key]
        public int TriggerId { get; set; }
        public TriggerEvent Event { get; set; }
        public double Value { get; set; }
        public bool SendOnFirstMatch { get; set; }

        public bool HookEnabled { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public bool TelegramEnabled { get; set; }
        public int TelegramId { get; set; }
        public Telegram Telegram { get; set; }
    }
}
