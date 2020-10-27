using AutoMapper;
using Venjix.DAL;
using Venjix.Models;

namespace Venjix.Infrastructure.Mapper
{
    public class UserPasswordResolver : IValueResolver<UserEditModel, User, string>
    {
        public string Resolve(UserEditModel source, User destination, string destMember, ResolutionContext context)
        {
            return BCrypt.Net.BCrypt.HashPassword(destMember);
        }
    }
}
