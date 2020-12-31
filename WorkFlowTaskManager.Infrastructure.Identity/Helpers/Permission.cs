namespace WorkFlowTaskManager.Infrastructure.Identity.Helpers
{
    public static class Permission
    {
        //User
        public const string ViewUser = "user.read";

        public const string AddUser = "user.create";
        public const string UpdateUser = "user.update";
        public const string DeleteUser = "user.delete";
        //Role
        public const string ViewRole = "user.read";

        public const string AddRole = "role.create";
        public const string UpdateRole = "role.update";
        public const string DeleteRole = "role.delete";

        //Audience
        public const string ViewAudience = "audience.read";

        public const string AddAudience = "audience.create";
        public const string UpdateAudience = "audience.update";
        public const string DeleteAudience = "audience.delete";

        //Internal Company
        public const string ViewInternalCompany = "internalcompany.read";

        public const string AddInternalCompany = "internalcompany.create";
        public const string UpdateInternalCompany = "internalcompany.update";
        public const string DeleteInternalCompany = "internalcompany.delete";
        //Internal Company contact
        public const string ViewInternalCompanyContact = "internalcompanycontact.read";

        public const string AddInternalCompanyContact = "internalcompanycontact.create";
        public const string UpdateInternalCompanyContact = "internalcompanycontact.update";
        public const string DeleteInternalCompanyContact = "internalcompanyCompany.delete";

        //Campaign
        public const string ViewCampaign = "campaign.read";

        public const string AddCampaign = "campaign.create";
        public const string UpdateCampaign = "campaign.update";
        public const string DeleteCampaign = "campaign.delete";

        //Contact To Campaign
        public const string ViewContactToCampaign = "contacttocampaign.read";

        public const string AddContactToCampaign = "contacttocampaign.create";
        public const string UpdateContactToCampaign = "contacttocampaign.update";
        public const string DeleteContactToCampaign = "contacttocampaign.delete";

        //Campaign Node
        public const string ViewCampaignNode = "campaignnode.read";

        public const string AddCampaignNode = "campaignnode.create";
        public const string UpdateCampaignNode = "campaignnode.update";
        public const string DeleteCampaignNode = "campaignnode.delete";
    }
}