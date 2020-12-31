using System;

namespace WorkFlowTaskManager.Application.DTO.User.Request
{
    public class SignUpUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}