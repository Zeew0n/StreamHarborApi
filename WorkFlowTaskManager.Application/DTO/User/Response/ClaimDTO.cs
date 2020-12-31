using System;

namespace WorkFlowTaskManager.Application.DTO.User.Response
{
    public class ClaimDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }
        public string Permissions { get; set; }
    }
}