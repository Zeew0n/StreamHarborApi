using System;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowTaskManager.Application.DTO
{
    public class CampaignDTO
    {
        [Key]
        public Guid Id { get; set; }

        public Guid InternalCompanyId { get; set; }

        public string Name { get; set; }

        public int NumberOfContactsEnrolled { get; set; }

        public int NumberOfSteps { get; set; }

        public int NumberOfUnsubscribes { get; set; }

        public bool IsActive { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime CreateDate { get; set; }


        public InternalCompanyDTO InternalCompany { get; set; }
    }
}