using System;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.Domain.Models
{
    public class RolePermissionMapping
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public AppRole Role { get; set; }
        public RolePermission Permission { get; set; }
    }
}