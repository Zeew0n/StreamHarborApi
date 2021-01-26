using System;

namespace WorkFlowTaskManager.Application.DTO.User.Response
{
    public class UserListDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}