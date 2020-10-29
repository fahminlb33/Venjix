using AutoMapper;
using Venjix.Infrastructure.DAL;
using Venjix.Models;

namespace Venjix.Infrastructure.Mappers
{
    public class UserPasswordResolver : IValueResolver<UserEditModel, User, string>
    {
        public string Resolve(UserEditModel source, User destination, string destMember, ResolutionContext context)
        {
            return BCrypt.Net.BCrypt.HashPassword(destMember);
        }
    }
}