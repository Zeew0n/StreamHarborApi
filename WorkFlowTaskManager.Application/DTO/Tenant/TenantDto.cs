using System;

namespace WorkFlowTaskManager.Application.DTO.Tenant
{
    public class TenantDto
    {
        public Guid TenantId { get; set; }
        public string OrganizationName { get; set; }

        public string OrganizationEmail { get; set; }

        public string SubDomain { get; set; }


    }
}
