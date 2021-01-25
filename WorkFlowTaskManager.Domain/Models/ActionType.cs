using System.Collections.Generic;

namespace WorkFlowTaskManager.Domain.Models
{
    public class ActionType : BaseEntity, ISoftDeletableEntity
    {
        public string Name { get; set; }

        //FK to Campaign Node
        public ICollection<CampaignNode> CampaignNodes { get; set; }
    }
}