using WorkFlowTaskManager.Application.DTO.Email;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Application.DTO.User.Response;
using WorkFlowTaskManager.Domain.Models;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string role);
        Task<bool> LoginAsync(AppUser appUser);

        Task<IdentityResult> CreateUserAsync(AppUser appUser, string role);

        Task<IdentityResult> UpdateUserAsync(AppUser oldUser, UpdateUserDTO userRegiterDTO);

        Task<IdentityResult> SignUpUserAsync(SignUpUserDTO signUpUserDTO);

        Task<bool> DeleteUserAsync(AppUser userDetail, Guid id);

        Task<IReadOnlyCollection<UserListDTO>> GetAllUsers();

        Task<UserDetailsDTO> GetUserDetailById(Guid userId);

        Task<AppUser> FindByEmailAsync(string email);

        Task<AppUser> FindByIdAsync(Guid id);

        Task<AppUser> FindByNameAsync(string name);

        Task<bool> CheckPasswordAsync(AppUser user, string password);

        Task<EmailDTO> GetEmailTokenWithContentAsync(AppUser appUser);

        Task<IdentityResult> ValidateEmailTokenAsync(AppUser appUser, string token);

        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token);
        Task<IdentityResult> ResetAsync(AppUser appUser, string token);
        Task<EmailDTO> GetEmailTokenWithContentResetAsync(AppUser appUser);

        Task<string> GenerateJWTToken(AppUser user);

    }
}