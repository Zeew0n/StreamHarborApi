using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Application.DTO.User;

namespace WorkFlowTaskManager.Application.Extensions
{
    public static class RefreshTokenExtension
    {
        public static RefreshTokenResponseDTO MapToRefreshTokenResponseDTO(this RefreshToken refToken)
        {
            return new RefreshTokenResponseDTO
            {
                Token = refToken.Token,
                ExpiryDate = refToken.ExpiryDate,
                IsExpired = refToken.IsExpired
            };
        }
    }
}