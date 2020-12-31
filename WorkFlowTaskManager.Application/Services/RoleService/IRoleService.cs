using WorkFlowTaskManager.Application.DTO.Role;
using WorkFlowTaskManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkFlowTaskManager.Application.Services
{
    public interface IRoleService
    {
        Task<RoleDto> CreateAsync(RoleDto role);
        Task<bool> UpdateAsync(RoleDto role);
        Task<bool> DeleteAsync(Guid Id);
        Task<List<RoleDto>> GetAllAsync(Guid? id = null);
        Task<RoleDto> GetByIdAsync(Guid id);

    }
}