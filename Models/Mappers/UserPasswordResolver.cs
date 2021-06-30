using AutoMapper;
using Venjix.Models.Entities;
using Venjix.Models.ViewModels;

namespace Venjix.Models.Mappers
{
    public class UserPasswordResolver : IValueResolver<UserEditModel, User, string>
    {
        public string Resolve(UserEditModel source, User destination, string destMember, ResolutionContext context)
        {
            return BCrypt.Net.BCrypt.HashPassword(destMember);
        }
    }
}