using System;
using System.Text.Json.Serialization;

namespace WorkFlowTaskManager.Application.DTO.User.Response
{
    public class AuthenticationResponseDTO
    {
        public string JWToken { get; set; }
        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }

        //For fetching Role to Control Menu
        public string RoleName { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string TenantId { get; set; }

        [JsonIgnore]
        public DateTime RefreshTokenExpiry { get; set; }
    }
}