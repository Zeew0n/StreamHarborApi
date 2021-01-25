using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowTaskManager.Domain.Models
{
    public class CampaignEventSchedule : BaseEntity, ISoftDeletableEntity
    {
        //Id of the Campaign
        public Guid CampaignId { get; set; }

        //Id of the Contact in the Campaign
        public Guid ContactId { get; set; }

        //Id of the Node Executed
        public Guid CampaignNodeId { get; set; }

        //Id of the Node in the previous step
        public Guid PreviousNodeId { get; set; }

        //Type of Campaign Action Type
        public Guid ActionTypeID { get; set; }

        //Time of scheduled execution
        public DateTime ScheduledExecuteTime { get; set; }

        //Has run or not
        public bool Executed { get; set; }

        //Time Executed
        public DateTime ExecutedTime { get; set; }

        public ActionType ActionType { get; set; }

        public Campaign Campaign { get; set; }

        [ForeignKey("CampaignNodeId")]
        public CampaignNode CampaignNode { get; set; }

        [ForeignKey("PreviousNodeId")]
        public CampaignNode PreviousCampaignNode { get; set; }

        public Contact Contact { get; set; }

        public ICollection<CampaignEventResult> CampaignEventResult { get; set; }
    }
}