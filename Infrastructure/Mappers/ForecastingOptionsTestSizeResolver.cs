using AutoMapper;
using Venjix.Infrastructure.AI;
using Venjix.Models;

namespace Venjix.Infrastructure.Mappers
{
    public class ForecastingOptionsTestSizeResolver : IValueResolver<ForecastModel, ForecastingOptions, float>
    {
        public float Resolve(ForecastModel source, ForecastingOptions destination, float destMember, ResolutionContext context)
        {
            return (float)source.TestSize / 100;
        }
    }
}
