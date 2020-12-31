using System.ComponentModel.DataAnnotations;

namespace WorkFlowTaskManager.Application.DTO.User.Role
{
    public class RoleRequestDTO
    {
        [Required]
        [MaxLength(25)]
        public string RoleName { get; set; }
    }
}