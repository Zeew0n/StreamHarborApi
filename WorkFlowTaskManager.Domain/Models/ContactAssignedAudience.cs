using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public class ContactAssignedAudience : BaseEntity, ISoftDeletableEntity
    {
        public Guid ContactId { get; set; }

        public Guid AudienceId { get; set; }

        public bool IsSubscribed { get; set; }

        public DateTime SubscribeDate { get; set; }

        public DateTime UnsubscribeDate { get; set; }

        //FK reference to Contact
        public Contact Contact { get; set; }

        //FK reference to Audience
        public Audience Audience { get; set; }
    }
}