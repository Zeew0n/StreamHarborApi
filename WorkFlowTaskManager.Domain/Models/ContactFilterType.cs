using System;
using System.Collections.Generic;

namespace WorkFlowTaskManager.Domain.Models
{
    public class ContactFilterType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<SendingProfileRule> SendingProfileRules { get; set; }

        //For ef
        private ContactFilterType() { }

        public ContactFilterType(
            Guid id,
            string name)
        {
            Id = id;
            Name = name;
        }
    }
}