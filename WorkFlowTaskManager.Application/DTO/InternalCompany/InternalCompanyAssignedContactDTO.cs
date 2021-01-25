using System;
using System.ComponentModel.DataAnnotations;
using WorkFlowTaskManager.Application.DTO.Contact;

namespace WorkFlowTaskManager.Application.DTO
{
    public class InternalCompanyAssignedContactDTO
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }

        public Guid InternalCompanyId {get; set; }

        public bool IsSubscribed { get; set; }

        public DateTime SubscribeDate { get; set; }

        public DateTime UnsubscribeDate { get; set; }

        //FK reference to Contact
        public ContactDTO Contact { get; set; }

        //FK reference to Companies
        public InternalCompanyDTO InternalCompany { get; set; }
    }
}