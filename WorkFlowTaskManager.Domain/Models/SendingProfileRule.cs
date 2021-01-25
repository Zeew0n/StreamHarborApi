using System;
using WorkFlowTaskManager.Domain.Enums;

namespace WorkFlowTaskManager.Domain.Models
{
    public class SendingProfileRule : BaseEntity, ISoftDeletableEntity
    {
        public Guid SendingProfileId { get; set; }

        public string RuleName { get; set; }

        public string FromFirstName { get; set; }

        public string FromLastName { get; set; }

        public string FromEmail { get; set; }

        public string ReplyToFirstName { get; set; }

        public string ReplyToLastName { get; set; }

        public string ReplyToEmail { get; set; }

        public Guid ContactFilterTypeId { get; set; }

        public ContactFilterValueType ContactFilterValueType { get; set; }

        public string ContactFilterValue { get; set; }

        public virtual SendingProfile SendingProfile { get; set; }

        public virtual ContactFilterType ContactFilterType { get; set; }
    }
}