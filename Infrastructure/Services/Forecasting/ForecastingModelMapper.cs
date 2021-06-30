using AutoMapper;
using Venjix.Models.ViewModels;

namespace Venjix.Infrastructure.Services.Forecasting
{
    public class ForecastingModelMapper : Profile
    {
        public ForecastingModelMapper()
        {
            CreateMap<ForecastModel, ForecastingOptions>()
                .ForMember(x => x.ConfidenceLevel, options => options.MapFrom<ForecastingOptionsConfidenceLevelResolver>())
                .ForMember(x => x.TestSize, options => options.MapFrom<ForecastingOptionsTestSizeResolver>());
        }
    }
}
