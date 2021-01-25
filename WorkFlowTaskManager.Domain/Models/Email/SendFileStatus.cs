namespace WorkFlowTaskManager.Domain.Models
{
    public class SendFileStatus : BaseEntity, ISoftDeletableEntity
    {
        public bool success { get; set; }

        public string message { get; set; }

        public string file_name { get; set; }

        public string file_id { get; set; }
    }
}