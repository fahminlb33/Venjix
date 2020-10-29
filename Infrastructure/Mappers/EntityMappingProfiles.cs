using AutoMapper;
using Venjix.Infrastructure.DAL;
using Venjix.Infrastructure.DTO;
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

            CreateMap<VenjixOptions, SettingsEditModel>();
            CreateMap<SettingsEditModel, VenjixOptions>();
        }
    }
}