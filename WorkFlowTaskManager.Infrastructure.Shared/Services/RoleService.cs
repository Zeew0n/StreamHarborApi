using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.Exceptions;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.Infrastructure.Shared.Services
{
    public class RoleService : IRoleServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a new role and assigns role access asynchronously.
        /// </summary>
        /// <param name="role">roleName and roleAccess list</param>
        /// <returns></returns>
        public async Task CreateAsync(AppRole role)
        {
            try
            {
                var entity = _unitOfWork.RoleRepository.Insert(role);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser userIdentity, string role)
        {
            try
            {
                var result = await _userManager.AddToRoleAsync(userIdentity, role);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(AppUser appUser)
        {
            try
            {
                var roleList = await GetUserRolesAsync(appUser);
                return await _userManager.RemoveFromRolesAsync(appUser, roleList);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task<IList<string>> GetUserRolesAsync(AppUser appUser) =>
    await _userManager.GetRolesAsync(appUser);

        public async Task<string> GetRoleNameByIdAsync(Guid roleId)
        {
            try
            {
                string result = await _unitOfWork.RoleRepository.GetAll(q => q.Id == roleId)
                                                            .Select(t => t.Name).SingleOrDefaultAsync();
                if (string.IsNullOrEmpty(result))
                    throw new ApiException("Provided role doesn't exist");

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
