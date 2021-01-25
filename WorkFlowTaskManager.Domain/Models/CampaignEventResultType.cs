namespace WorkFlowTaskManager.Domain.Models
{
    public class CampaignEventResultType : BaseEntity, ISoftDeletableEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string EventType { get; set; }
    }
}