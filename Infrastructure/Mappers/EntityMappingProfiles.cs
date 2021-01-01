using AutoMapper;
using Venjix.Infrastructure.AI;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.Services;
using Venjix.Models;

namespace Venjix.Infrastructure.Mappers
{
    public class EntityMappingProfiles : Profile
    {
        public EntityMappingProfiles()
        {
            CreateMap<User, UserEditModel>()
                .ForMember(x => x.Password, options => options.Ignore());
            CreateMap<UserEditModel, User>()
                .ForMember(x => x.Password, options => options.MapFrom<UserPasswordResolver>());

            CreateMap<Sensor, SensorEditModel>();
            CreateMap<SensorEditModel, Sensor>();

            CreateMap<Webhook, WebhookEditModel>();
            CreateMap<WebhookEditModel, Webhook>();

            CreateMap<Trigger, TriggerEditModel>();
            CreateMap<TriggerEditModel, Trigger>();

            CreateMap<VenjixOptions, SettingsModel>();
            CreateMap<SettingsModel, VenjixOptions>();

            CreateMap<ForecastModel, ForecastingOptions>()
                .ForMember(x => x.ConfidenceLevel, options => options.MapFrom<ForecastingOptionsConfidenceLevelResolver>())
                .ForMember(x => x.TestSize, options => options.MapFrom<ForecastingOptionsTestSizeResolver>());
            
        }
    }
}