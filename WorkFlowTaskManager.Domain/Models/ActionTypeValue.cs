using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public class ActionTypeValue : BaseEntity, ISoftDeletableEntity
    {
        public Guid CampaignNodeId { get; set; }

        public Guid? SendingProfileId { get; set; }

        public Guid? EmailId { get; set; }

        public int? DayDelay { get; set; }

        public TimeSpan? TimeToSend { get; set; }

        public string WebhookURL { get; set; }

        public Guid? NewCampaignID { get; set; }

        //FK References

        public CampaignNode CampaignNode { get; set; }

        public SendingProfile SendingProfile { get; set; }

        public Email Email { get; set; }

        public Campaign Campaign { get; set; }
    }
}