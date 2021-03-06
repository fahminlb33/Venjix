﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Venjix.Models.Entities
{
    public class Webhook
    {
        [Key]
        public int WebhookId { get; set; }

        public string Name { get; set; }
        public string UriFormat { get; set; }
        public string BodyFormat { get; set; }
        public bool JsonBody { get; set; }
        public string Method { get; set; }

        public List<Trigger> Triggers { get; set; }
    }
}