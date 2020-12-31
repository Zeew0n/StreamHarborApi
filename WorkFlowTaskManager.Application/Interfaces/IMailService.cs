using WorkFlowTaskManager.Application.DTO.Email;

using System.Threading.Tasks;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailDTO emailDTO);
    }
}