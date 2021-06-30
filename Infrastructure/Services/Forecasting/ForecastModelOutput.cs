namespace Venjix.Infrastructure.Services.Forecasting
{
    public class ForecastModelOutput
    {
        public float[] ForecastedValues { get; set; }

        public float[] LowerBounds { get; set; }

        public float[] UpperBounds { get; set; }
    }
}
