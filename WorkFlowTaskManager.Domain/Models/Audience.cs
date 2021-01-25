using System;
using System.Collections.Generic;

namespace WorkFlowTaskManager.Domain.Models
{
    public class Audience : BaseEntity, ISoftDeletableEntity
    {
        public string Name { get; set; }
        public Guid InternalCompanyId { get; set; }
        public virtual ICollection<ContactAssignedAudience> ContactAssignedAudience { get; set; }
        public virtual ICollection<AudienceAssignedCampaign> AudienceAssignedCampaign { get; set; }
        public virtual InternalCompany InternalCompany { get; set; }
    }
}