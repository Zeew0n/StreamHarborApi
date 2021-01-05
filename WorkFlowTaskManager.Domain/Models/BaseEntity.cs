using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public abstract class BaseEntity
    {
        //This is a test comment to check
        //This is test 2
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}