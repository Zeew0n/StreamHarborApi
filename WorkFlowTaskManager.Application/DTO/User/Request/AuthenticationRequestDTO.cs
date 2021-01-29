﻿using System;

namespace WorkFlowTaskManager.Application.DTO.User.Request
{
    public class AuthenticationRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid TenantId { get; set; }
    }
}