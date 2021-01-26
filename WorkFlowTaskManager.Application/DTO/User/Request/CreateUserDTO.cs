using System;

namespace WorkFlowTaskManager.Application.DTO.User.Request
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string UserName { get; set; }
        public string  Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int MyProperty { get; set; }
        public string OrganizationName { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string SubDomain { get; set; }
        public string RoleName { get; set; }
        public Guid RoleId { get; set; }
        public Guid TenantId { get; set; }


    }
}