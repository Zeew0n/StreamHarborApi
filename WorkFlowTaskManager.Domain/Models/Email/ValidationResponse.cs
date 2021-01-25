using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public class ValidationResponse : BaseEntity, ISoftDeletableEntity
    {
        public string file_id { get; set; }

        public string file_name { get; set; }

        public DateTime upload_date { get; set; }

        public string Status { get; set; }
    }
}