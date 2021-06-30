namespace Venjix.Infrastructure.Services.Forecasting
{
    public class ForecastingOptions
    {
        public float TestSize { get; set; }
        public int WindowSize { get; set; }
        public int SeriesLength { get; set; }
        public int Horizon { get; set; }
        public float ConfidenceLevel { get; set; }
    }
}
