using System;
using System.Collections.Generic;
using System.Security;

namespace WorkFlowTaskManager.Application.DTO.Role
{
    public class RoleDto
    {
        public string Access { get; set; }
        public  List<Guid> Premission { get; set; }
        public string RoleName { get; set; }
        public Guid RoleId { get; set; }
    }
}