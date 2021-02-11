namespace Venjix.Infrastructure.Authentication
{
    public static class Roles
    {
        public const int SuperadminUserId = 1;

        public const string Admin = "Admin";
        public const string User = "User";
        public const string AdminOrUser = "Admin, User";

        public static readonly string[] AllRoles = { Admin, User };

        public static bool CanChangeRole(string role, int userId)
        {
            return role == Admin && userId != SuperadminUserId;
        }
    }
}