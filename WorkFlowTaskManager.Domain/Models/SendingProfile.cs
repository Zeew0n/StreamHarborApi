using System;
using System.Collections.Generic;

namespace WorkFlowTaskManager.Domain.Models
{
    public class SendingProfile : BaseEntity, ISoftDeletableEntity
    {
        public Guid InternalCompanyId { get; set; }

        public string Name { get; set; }

        public virtual InternalCompany InternalCompany { get; set; }

        public virtual ICollection<ActionTypeValue> ActionTypeValues { get; set; }

        public virtual ICollection<SendingProfileRule> SendingProfileRules { get; set; }
    }
}