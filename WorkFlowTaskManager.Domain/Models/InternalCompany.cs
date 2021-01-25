using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowTaskManager.Domain.Models
{
    [Table("InternalCompany")]
    public class InternalCompany : BaseEntity, ISoftDeletableEntity
    {
        /*
         * Core Internal Company Info
         */
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }

        //
        //public ICollection<InternalCompanyAssignedContact>? InternalCompanyAssignedContacts { get; set; }

        //
        //public ICollection<Campaign>? Campaigns { get; set; }
        //public virtual ICollection<Audience>? Audiences { get; set; }
    }
}