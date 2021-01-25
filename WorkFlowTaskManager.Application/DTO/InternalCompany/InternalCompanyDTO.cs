using System;

namespace WorkFlowTaskManager.Application.DTO
{
    public class InternalCompanyDTO
    {
        /*
         * IDs and Keys
         */
        public Guid Id { get; set; }

        /*
         * Core Internal Company Info
         */
        public string Name { get; set; }

        public bool IsActive { get; set; }

        /*
         * Date Management
         */
        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        //
        //public IEnumerable<InternalCompanyAssignedContactDTO> InternalCompanyAssignedContacts { get; set; }

        //
       // public IEnumerable<CampaignDTO> Campaigns { get; set; }
    }
}