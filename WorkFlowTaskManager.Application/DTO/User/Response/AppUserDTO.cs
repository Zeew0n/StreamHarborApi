using WorkFlowTaskManager.Application.DTO.User;

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace WorkFlowTaskManager.Application.DTO.User.Response
{
    public class AppUserDTO
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public bool IsAdmin { get; set; }
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public RefreshTokenResponseDTO RefreshToken { get; set; }
        public List<string> Permissions { get; set; }
    }
}