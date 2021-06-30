using System;
using System.Collections.Generic;

namespace Venjix.Models.Dtos
{
    public class HealthCheckDto
    {
        public string Status { get; set; }
        public IEnumerable<IndividualHealthCheckDto> HealthChecks { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}