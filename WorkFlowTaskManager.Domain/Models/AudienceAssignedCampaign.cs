using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public class AudienceAssignedCampaign : BaseEntity, ISoftDeletableEntity
    {
        public Guid CampaignId { get; set; }

        public Guid AudienceId { get; set; }

        public bool IsAssigned { get; set; }

        //FK reference to Contact
        public Campaign Campaign { get; set; }

        //FK reference to Campaign
        public Audience Audience { get; set; }
    }
}