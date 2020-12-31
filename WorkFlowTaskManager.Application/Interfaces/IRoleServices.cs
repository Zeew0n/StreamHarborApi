using WorkFlowTaskManager.Domain.Models;

using Microsoft.AspNetCore.Identity;

using System;
using System.Threading.Tasks;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface IRoleServices
    {
        Task CreateAsync(AppRole role);

        Task<IdentityResult> AddToRoleAsync(AppUser userIdentity, string role);

        Task<IdentityResult> RemoveFromRoleAsync(AppUser appUser);

        Task<string> GetRoleNameByIdAsync(Guid roleId);
    }
}