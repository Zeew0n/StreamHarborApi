using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTaskManager.Domain.Models.User
{
    public class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        //Session
        public int TenantId { get; set; }

    }
}
