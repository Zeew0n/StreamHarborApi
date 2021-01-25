using System;
using System.Collections.Generic;

namespace WorkFlowTaskManager.Domain.Models
{
    public class Campaign : BaseEntity, ISoftDeletableEntity
    {
        public Guid InternalCompanyId { get; set; }

        public string Name { get; set; }

        public int NumberOfContactsEnrolled { get; set; }

        public int NumberOfSteps { get; set; }

        public int NumberOfUnsubscribes { get; set; }

        public bool IsActive { get; set; }

        public ICollection<AudienceAssignedCampaign> AudienceAssignedCampaign { get; set; }

        public ICollection<CampaignEventSchedule> CampaignEventSchedule { get; set; }

        public ICollection<CampaignNode> CampaignNodes { get; set; }

        public InternalCompany InternalCompany { get; set; }
    }
}