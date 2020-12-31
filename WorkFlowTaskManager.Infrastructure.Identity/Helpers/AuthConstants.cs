namespace WorkFlowTaskManager.Infrastructure.Identity.Helpers
{
    public static class AuthConstants
    {
        public const string UnauthorizedMessage = "You don't have permission to access this resource.";
        public const string JwtId = "id", RoleAccess = "role_access", LoginId = "loginId", Role = "role", AuthKey = "JwtPrivateKey", DBConnection = "DBConnection";

        public const string UserName = "username", RoleId = "roleid", Permissions = "permissions", IsAdmin = "admin";
    }
}