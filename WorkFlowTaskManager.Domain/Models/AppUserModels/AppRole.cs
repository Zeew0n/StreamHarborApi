using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTaskManager.Domain.Models.AppUserModels
{
    public class AppRole : IdentityRole<Guid>, ISoftDeletableEntity
    {
        public ICollection<RolePermissionMapping> RolePermissions { get; set; }
    }
}
