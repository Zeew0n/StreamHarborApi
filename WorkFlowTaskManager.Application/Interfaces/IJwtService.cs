using WorkFlowTaskManager.Application.DTO.User.Response;

using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface IJwtService
    {
        ClaimsIdentity GenerateClaimsIdentity(ClaimDTO claimDTO);
        Task<AuthenticationResponseDTO> GenerateJwt(AppUserDTO userDetails);
    }
}