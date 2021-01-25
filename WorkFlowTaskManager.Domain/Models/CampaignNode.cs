using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowTaskManager.Domain.Models
{
    public class CampaignNode : BaseEntity, ISoftDeletableEntity
    {
        public Guid CampaignId { get; set; }

        public Guid? ActionTypeId { get; set; }

        public Guid PreviousNodeId { get; set; }

        public string NodeKey { get; set; }

        public string PreviousNodeKey { get; set; }

        public string Name { get; set; }

        public int ReferenceCount { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }

        [ForeignKey("ActionTypeId")]
        public ActionType ActionType { get; set; }

        public ICollection<ActionTypeValue> ActionTypeValues { get; set; }

        public ICollection<CampaignEventSchedule> CampaignEventSchedule { get; set; }
    }
}