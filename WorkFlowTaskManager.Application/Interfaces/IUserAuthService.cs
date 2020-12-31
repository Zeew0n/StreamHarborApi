using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Application.DTO.User.Response;

using System.Threading.Tasks;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface IUserAuthService
    {
        Task<AuthenticationResponseDTO> AuthenticateAsync(AuthenticationRequestDTO request);

        Task<AuthenticationResponseDTO> RefreshTokenAsync(string refreshToken);

        Task<bool> RevokeTokenAsync(string token);
    }
}