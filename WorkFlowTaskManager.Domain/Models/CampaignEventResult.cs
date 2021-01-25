using System;

namespace WorkFlowTaskManager.Domain.Models
{
    public class CampaignEventResult : BaseEntity, ISoftDeletableEntity
    {
        /*
         * Custom Variables Used For Tracking Events
         */
        public Guid CampaignEventScheduleId { get; set; }

        //Id of the Contact in the Campaign
        public Guid ContactId { get; set; }

        //Id of the Campaign
        public Guid CampaignId { get; set; }

        //Id of the Node Executed
        public Guid CampaignNodeId { get; set; }

        //Type of Campaign Action Type
        public Guid ActionTypeId { get; set; }

        //Value Supplied to Action Type
        public string ActionTypeValue { get; set; }

        //Result Value
        public Guid CampaignEventResultTypeId { get; set; }

        //Id of the SendGridEmailTemplate (If sending Email)
        public string SendGridEmailTemplateId { get; set; }

        //Id of the Sending Profile Rule
        public Guid SendingProfileRuleId { get; set; }

        /*
         * Response Varibles - Sendgrid
         */

        //The email address of the recipiant
        public string Email { get; set; }

        //Unix timestamp of the event
        public double TimeStamp { get; set; }

        //Unique ID attached to the message by the originating system
        public string SmtpId { get; set; }

        //The event type
        public string Event { get; set; } //Possible values: processed, dropped, delivered, deferred, bounce, open, click, spam report, unsubscribe, group unsubscribe, and group resubscribe

        //Custom tag for organizing emails
        public string Category { get; set; }

        //A unique ID to this event that you can use for deduplication purposes
        public string SG_Event_Id { get; set; }

        //A unique, internal SendGrid ID for the message. The first half of this ID is pulled from the smtp-id.
        public string SG_Message_Id { get; set; }

        //Any sort of error response returned by the receiving server that describes the reason this event type was triggered.
        public string Response { get; set; }

        //The number of times SendGrid has attempted to deliver this message.
        public int Attempt { get; set; }

        //The user agent responsible for the event. This is usually a web browser.
        public string UserAgent { get; set; }

        //The IP address used to send the email. For open and click events, it is the IP address of the recipient who engaged with the email.
        public string IP { get; set; }

        //The URL where the event originates. For click events, this is the URL clicked on by the recipient.
        public string URL { get; set; }

        //The full text of the HTTP response error returned from the receiving server.
        public string Reason { get; set; }

        //status code string. Corresponds to HTTP status code.
        public string Status { get; set; }

        //unsubscribe group id
        public int ASM_Group_Id { get; set; }

        /*
         * Response Variables - Web Hook
         */

        public string WebhookURL { get; set; }

        public string WebhookStatus { get; set; }

        /*
         * Response Variables -  Push to CRM
         */
        public string CRMStatus { get; set; }

        /*
         * Response Variables - Push for Call
         */
        public string PhoneNumber { get; set; }

        /*
         * Response Variables - Move to Campaign
         */

        /*
         * FK References
         */

        public ActionType ActionType { get; set; }

        public Campaign Campaign { get; set; }

        public CampaignNode CampaignNode { get; set; }

        public Audience Audience { get; set; }

        public Contact Contact { get; set; }

        public SendingProfileRule SendingProfileRule { get; set; }

        public CampaignEventSchedule CampaignEventSchedule { get; set; }

        public CampaignEventResultType CampaignEventResultType { get; set; }
    }
}