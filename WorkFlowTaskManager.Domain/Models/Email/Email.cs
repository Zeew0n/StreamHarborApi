using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public class Email : BaseEntity, ISoftDeletableEntity
    {
        public Guid CompanyId { get; set; }

        public string SendGridTemplateID { get; set; }

        public string SubjectLine { get; set; }
    }
}