using System;

namespace WorkFlowTaskManager.Application.DTO.User
{
    public class RefreshTokenResponseDTO
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
    }
}