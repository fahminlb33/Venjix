using AutoMapper;
using Venjix.Models.ViewModels;

namespace Venjix.Infrastructure.Services.Forecasting
{
    public class ForecastingOptionsTestSizeResolver : IValueResolver<ForecastModel, ForecastingOptions, float>
    {
        public float Resolve(ForecastModel source, ForecastingOptions destination, float destMember, ResolutionContext context)
        {
            return (float)source.TestSize / 100;
        }
    }
}
