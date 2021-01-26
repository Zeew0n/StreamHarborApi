using System;
using System.Collections.Generic;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.Domain.Models.Tenant
{
    public class TenantInformation:BaseEntity
    {

         public Guid TenantId { get; set; } = Guid.NewGuid();
        public string OrganizationName { get; set; }

        public string OrganizationEmail { get; set; }

        public string SubDomain { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
    }
}
