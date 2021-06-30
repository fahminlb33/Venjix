using AutoMapper;
using Venjix.Models.ViewModels;

namespace Venjix.Infrastructure.Services.Options
{
    public class VenjixOptionsMapper : Profile
    {
        public VenjixOptionsMapper()
        {
            CreateMap<VenjixOptions, SettingsModel>();
            CreateMap<SettingsModel, VenjixOptions>();
        }
    }
}
