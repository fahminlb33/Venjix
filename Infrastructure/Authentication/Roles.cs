namespace Venjix.Infrastructure.Authentication
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string AdminOrUser = "Admin, User";

        public static readonly string[] AllRoles = { Admin, User };
    }
}
