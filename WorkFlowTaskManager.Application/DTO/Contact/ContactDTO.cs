using System;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowTaskManager.Application.DTO.Contact
{
    public class ContactDTO
    {
        /*
         * IDs and Keys
         */
        /*
         * IDs and Keys
         */

        [Key]
        public Guid Id { get; set; }

        public string SourceId { get; set; }

        /*
         * Core Contact Info
         */
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string PrimaryEmail { get; set; }

        public string PrimaryPhoneNumber { get; set; }

        public string PrimaryAddress { get; set; }

        public string PrimaryCity { get; set; }

        public string PrimaryState { get; set; }

        public string PrimaryZip { get; set; }

        public string CompanyName { get; set; }

        public string WebsiteAddress { get; set; }

        public string RevenueSize { get; set; }

        public string NumberOfEmployees { get; set; }

        public string CompanyPhoneNumber { get; set; }

        /*
         * Company Info
         */
        public string CompanyAddress { get; set; }

        public string CompanyCity { get; set; }

        public string CompanyState { get; set; }

        public string CompanyZip { get; set; }

        public string CompanyAddress2 { get; set; }

        public string CompanyCity2 { get; set; }

        public string CompanyState2 { get; set; }

        public string CompanyZip2 { get; set; }

        public string CompanyAddress3 { get; set; }

        public string CompanyCity3 { get; set; }

        public string CompanyState3 { get; set; }

        public string CompanyZip3 { get; set; }

        /*
         * Industry Info
         */
        public string IndustryName { get; set; }

        public string SIC_Code2 { get; set; }

        public string SIC_Code4 { get; set; }

        public string SIC_Code6 { get; set; }

        public string SIC_Code8 { get; set; }

        public string SIC_Code2_Description { get; set; }

        public string SIC_Code4_Description { get; set; }

        public string SIC_Code6_Description { get; set; }

        public string SIC_Code8_Description { get; set; }

        public string NAICS_Code1 { get; set; }

        public string NAICS_Code2 { get; set; }

        public string NAICS_Code3 { get; set; }

        public string NAICS_Code4 { get; set; }

        public string NAICS_Code1_Description { get; set; }

        public string NAICS_Code2_Description { get; set; }

        public string NAICS_Code3_Description { get; set; }

        public string NAICS_Code4_Description { get; set; }

        /*
         * Email Validation
         */
        public bool EmailValid { get; set; }

        public bool EmailInvalid { get; set; }

        public string EmailStatus { get; set; }

        public string EmailSubStatus { get; set; }

        public bool CatchAll { get; set; }
        public bool SpamTrap { get; set; }

        public bool Abuse { get; set; }

        public bool DoNotMail { get; set; }

        public bool Unknown { get; set; }

        public bool AliasAddress { get; set; }

        public bool AntispamSystem { get; set; }

        public bool DoesNotAcceptMail { get; set; }

        public bool ExceptionOccurred { get; set; }

        public bool FailedSMTPConnection { get; set; }

        public bool ForcibleDisconnect { get; set; }

        public bool GlobalSuppression { get; set; }

        public bool GreyListed { get; set; }

        public bool LeadingPeriodRemoved { get; set; }

        public bool MailServerDidNotRespond { get; set; }

        public bool MailServerTemporaryError { get; set; }

        public bool MailboxQuotaExceeded { get; set; }

        public bool MailboxNotFound { get; set; }

        public bool PossibleTrap { get; set; }

        public bool PossibleTypo { get; set; }

        public bool RoleBased { get; set; }

        public bool TimeoutExceeded { get; set; }

        public bool UnroutableIPAddress { get; set; }

        public bool Disposable { get; set; }

        public bool Toxic { get; set; }

        public string Local { get; set; }

        public string Host { get; set; }

        public string Suggestion { get; set; }

        public bool HasValidAddressSyntax { get; set; }

        public bool HasMXRecord { get; set; }

        public string MXRecordValue { get; set; }

        public bool IsFreeEmail { get; set; }

        public double DomainAge { get; set; }

        public string SMTPProvider { get; set; }

        public string EmailFirstName { get; set; }

        public string EmailLastName { get; set; }

        public string Gender { get; set; }

        public string IPCountry { get; set; }

        public string IPRegion { get; set; }

        public string IPCity { get; set; }

        public string IPZip { get; set; }

        public DateTime? LastSentDate { get; set; }

        /*
         * Score
         */
        public double? TotalScore { get; set; }

        public double? EmailScore { get; set; }

        public double? PhoneScore { get; set; }

        public double? CompletenessScore { get; set; }

        public double? SocialScore { get; set; }

        public double? SocialProfileCount { get; set; }

        /*
         * Phone Validation
         */
        public bool? PhoneValid { get; set; }

        public bool? PhoneInvalid { get; set; }

        public bool? ContactIsValidPhoneSyntax { get; set; }

        public bool? ContactHasValidPhoneNumber { get; set; }

        public bool? CompanyIsValidPhoneSyntax { get; set; }

        public bool? CompanyHasValidPhoneNumber { get; set; }

        /*
         * Standard Columns
         */
        public Guid UpdatedById { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public Guid CreatedById { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public Guid? DeletedById { get; set; } = null;

        public DateTime? DeleteDate { get; set; } = null;
    }
}