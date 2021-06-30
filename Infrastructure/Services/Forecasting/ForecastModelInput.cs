using System;

namespace Venjix.Infrastructure.Services.Forecasting
{
    public class ForecastModelInput
    {
        public DateTime RecordTime { get; set; }

        public float Value { get; set; }
    }
}
