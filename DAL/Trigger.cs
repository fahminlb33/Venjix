using System.ComponentModel.DataAnnotations;

namespace Venjix.DAL
{
    public class Trigger
    {
        [Key]
        public int TriggerId { get; set; }
        public TriggerEvent Event { get; set; }
        public TriggerTarget Target { get; set; }
        public double Value { get; set; }
        public int TargetId { get; set; }
        public bool ContinuousSend { get; set; }
    }
}
