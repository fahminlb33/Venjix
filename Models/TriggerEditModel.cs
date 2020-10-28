using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.Helpers;

namespace Venjix.Models
{
    public class TriggerEditModel
    {
        public int TriggerId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        public TriggerEvent Event { get; set; }
        public TriggerTarget Target { get; set; }
        public double Value { get; set; }
        public bool ContinuousSend { get; set; }

        [Required]
        public int? SensorId { get; set; }

        [RequiredIf(nameof(Target), TriggerTarget.Webhook)]
        public int? WebhookId { get; set; }
        public bool IsEdit { get; set; }

        public List<SelectListItem> Events { get; set; }
        public List<SelectListItem> Targets { get; set; }
        public List<SelectListItem> Sensors { get; set; }
        public List<SelectListItem> Webhooks { get; set; }
    }
}
