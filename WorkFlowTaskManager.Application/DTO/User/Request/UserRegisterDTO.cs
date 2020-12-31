using System;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowTaskManager.Application.DTO.User.Request
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}