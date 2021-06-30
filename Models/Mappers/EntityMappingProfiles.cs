using AutoMapper;
using Venjix.Models.Entities;
using Venjix.Models.ViewModels;

namespace Venjix.Models.Mappers
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
        }
    }
}