using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Email;
using WorkFlowTaskManager.Application.Interfaces;

namespace StreamHarborAPI.Services
{

    public class SendGridMailService : IMailService
    {
        private IConfiguration _configuration;
        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailDTO emailDTO)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("abhattarai@devfinity.io", "JWT Implementation");
            var to = new EmailAddress(emailDTO.To);
            var msg = MailHelper.CreateSingleEmail(from,to,emailDTO.Subject,emailDTO.Body,emailDTO.Body);
            var response = await client.SendEmailAsync(msg);
        }


    }
}
