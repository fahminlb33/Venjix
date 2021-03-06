﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Venjix.Models.Entities
{
    public class Recording
    {
        [Key]
        public int RecordingId { get; set; }

        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}