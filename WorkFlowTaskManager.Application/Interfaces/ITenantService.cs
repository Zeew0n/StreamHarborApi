using System.Collections.Generic;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Tenant;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface ITenantService
    {
        Task<TenantDto> CreateAsync(TenantDto model);

        //Task<UserLoginResponse> LoginAsync(AppUser appUser, string password);
        // Task<UserLoginResponse> LoginUserAsync(AppUser appUser);

        //Task<IdentityResult> CreateUserAsync(AppUser appUser, string role);

        //Task<IdentityResult> UpdateTenantAsync(AppUser oldUser, UpdateUserDTO userRegiterDTO);

        //Task<IdentityResult> SignUpUserAsync(SignUpUserDTO signUpUserDTO);

        //Task<bool> DeleteTenantAsync(AppUser userDetail, Guid id);

        Task<IEnumerable<TenantDto>> GetAllTenants();

        //Task<UserDetailsDTO> GetUserDetailById(Guid userId);

        //Task<AppUser> FindByEmailAsync(string email);

        //Task<AppUser> FindByIdAsync(Guid id);

        //Task<AppUser> FindByNameAsync(string name);

        //Task<bool> CheckPasswordAsync(AppUser user, string password);

        //Task<EmailDTO> GetEmailTokenWithContentAsync(AppUser appUser);

        ////Task<bool> ValidateEmailTokenAsync(AppUser appUser, string token);

        //Task<bool> ValidateEmailTokenAsync(AppUser appUser);


        //Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password);
        //Task<IdentityResult> ResetAsync(AppUser appUser, string token, string password);
        //Task<EmailDTO> GetEmailTokenWithContentResetAsync(AppUser appUser);

        //Task<string> GenerateJWTToken(AppUser user);

        //Task<bool> ConfirmUserAsync(AppUser appUser, string token);


    }
}
