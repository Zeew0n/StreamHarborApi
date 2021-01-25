namespace WorkFlowTaskManager.Domain.Models.User
{
    public class UserSignUp: BaseEntity
    {
        public string UserId { get; set; }
        //public string UserName { get; set; }
        //For new User Creation Only Email, TenantId and Role
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        //AutoIncrement
        public string TenantId { get; set; }


        //default 1
        public string RoleId { get; set; }

        //database
        public string DatabaseHost { get; set; }

        //User Creation Goes to TempDB

    }
}
