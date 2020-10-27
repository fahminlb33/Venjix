using AutoMapper;
using Venjix.DAL;
using Venjix.Models;

namespace Venjix.Infrastructure.Mapper
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
        }
    }
}
