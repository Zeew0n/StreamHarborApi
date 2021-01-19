using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Email;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Application.DTO.User.Response;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Infrastructure.Identity.Helpers;
using WorkFlowTaskManager.Infrastructure.Shared.Helpers;

namespace WorkFlowTaskManager.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #region Command Methods

        public async Task<IdentityResult> CreateAsync(AppUser appUser, string role)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.Email;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserNameDetail = await FindByNameAsync(userName);
                if (oldUserEmailDetail != null)
                    throw new Exception($"DuplicateEmail: Email {email} is already taken.");
                if (oldUserNameDetail != null)
                    throw new Exception($"DuplicateUserName: UserName {userName} is already taken.");

                return await SaveUserAsync(appUser, role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserLoginResponse> LoginAsync(AppUser appUser, string password)
        {
            try
            {
                var result = await _userManager.CheckPasswordAsync(appUser, password);
                if (!result)

                return new UserLoginResponse
                {
                    Message = "Invalid Password",
                    IsSuccess = false,
                };

                //To Generate Token
                var token = GenerateJWTToken(appUser);
                string tokenValue = token.Result;
                return new UserLoginResponse
                {
                    Message = tokenValue,
                    IsSuccess = true

                };


                //return await LoginUserAsync(appUser);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<IdentityResult> CreateUserAsync(AppUser appUser, string role)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.UserName;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserNameDetail = await FindByNameAsync(userName);
                if (oldUserEmailDetail != null)
                    throw new Exception($"DuplicateEmail: Email {email} is already taken.");

                if (oldUserNameDetail != null)
                    throw new Exception($"DuplicateUserName: UserName {userName} is already taken.");

                return await SaveUserAsync(appUser, role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser appUser,string token)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.UserName;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserUsername = await FindByUsernameAsync(email);

                if (oldUserEmailDetail == null)
                    throw new Exception($"Email not available.");

                if (oldUserUsername == null)
                    throw new Exception($"Username not available.");

                return await ResetAsync(appUser,token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> ResetAsync(AppUser appUser,string token)
        {
            try
            {
                Guid id = appUser.Id;
                AppUser user = await FindByIdAsync(id);
                var  userResult = await _userManager.ResetPasswordAsync(user,token,appUser.Password);
                return userResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Confirm User Email and Reset Password

        public async Task<IdentityResult> ConfirmUserAsync(AppUser appUser, string token)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.UserName;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserUsername = await FindByUsernameAsync(email);

                if (oldUserEmailDetail == null)
                    throw new Exception($"Email not available.");

                if (oldUserUsername == null)
                    throw new Exception($"Username not available.");

                return await ConfirmAsync(appUser, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> ConfirmAsync(AppUser appUser, string token)
        {
            try
            {
                Guid id = appUser.Id;
                AppUser user = await FindByIdAsync(id);
                var userpasswordResult = await _userManager.ResetPasswordAsync(user, token, appUser.Password);
                var userResult = await _userManager.ConfirmEmailAsync(user, token);
                return userResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<IdentityResult> SaveUserAsync(AppUser appUser, string role)
        {
            string email = appUser.Email;
            //For TenantId
            Guid obj = Guid.NewGuid();

            if (!string.IsNullOrEmpty(email))
            {
                appUser.IsAdmin = role.ToUpper() == DesignationAndRoleConstants.Admin;
                appUser.TenantId = obj;
            }

            var userResult = await _userManager.CreateAsync(appUser,appUser.Password);
            return userResult;
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser oldUser, UpdateUserDTO userRegiterDTO)
        {
            try
            {

                string username = userRegiterDTO.UserName;
                AppUser oldUserName = await FindByUsernameAsync(username);
                    if (oldUserName != null)
                        throw new Exception($"DuplicateUsername: Username {username} is already taken.");

                    return await EditUserAsync(oldUser, userRegiterDTO);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<IdentityResult> EditUserAsync(AppUser oldUser, UpdateUserDTO userRegiterDTO)
        {
            oldUser.UserName = userRegiterDTO.UserName.ToLower();
            oldUser.Password = userRegiterDTO.Password;
            var userResult = await _userManager.UpdateAsync(oldUser);
            return userResult;
        }

        public async Task<IdentityResult> SignUpUserAsync(SignUpUserDTO signUpUserDTO)
        {
            try
            {
                var user = new AppUser
                {
                    Email = signUpUserDTO.UserName.Trim(),
                    UserName = signUpUserDTO.UserName.Trim(),
                    IsSuperAdmin= true,
                };

                var userResult = await _userManager.CreateAsync(user,signUpUserDTO.Password);
                return userResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUserAsync(AppUser userDetail, Guid id)
        {
            try
            {
                if (userDetail == null)
                {
                    throw new Exception("Provided user doesn't exists.");
                }

                _unitOfWork.UserRepository.Delete(userDetail);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion Command Methods

        #region GetMethods

        /// <summary>
        /// Verify password asynchronously.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> CheckPasswordAsync(AppUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        /// <summary>
        /// Gets user data by name asynchronously.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AppUser> FindByNameAsync(string name)
        {
            return await _unitOfWork.UserRepository.GetAll(x => x.UserName == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets user data by email asynchronously.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetAll(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<AppUser> FindByUsernameAsync(string username)
        {
            return await _unitOfWork.UserRepository.GetAll(x => x.UserName == username).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Gets user data by id asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppUser> FindByIdAsync(Guid id) => await _userManager.FindByIdAsync(id.ToString());

        public async Task<IReadOnlyCollection<UserListDTO>> GetAllUsers()
        {
            var result = await (from user in _unitOfWork.UserRepository.GetAllIgnoreQueryFilter()
                                join userRole in _unitOfWork.UserRoleRepository.GetAll() on user.Id equals userRole.UserId
                                join role in _unitOfWork.RoleRepository.GetAll() on userRole.RoleId equals role.Id
                                select new
                                {
                                    user.Id,
                                    user.UserName,
                                    user.Email,
                                    user.FirstName,
                                    user.LastName,
                                    user.CreatedDate,
                                    Role = role.Name,
                                    EmailConfirmmed = user.EmailConfirmed,
                                    IsDeleted = EF.Property<bool>(user, "IsDeleted")
                                }).OrderByDescending(q => q.CreatedDate).ToListAsync();
            return result.Select(user => new UserListDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Status = user.IsDeleted ? UserConstants.Deleted : (user.EmailConfirmmed ? UserConstants.Active : UserConstants.InActive)
            }).ToList();
        }

        public async Task<UserDetailsDTO> GetUserDetailById(Guid userId)
        {
            return await (from user in _unitOfWork.UserRepository.GetAll()
                          join userRole in _unitOfWork.UserRoleRepository.GetAll() on user.Id equals userRole.UserId
                          join role in _unitOfWork.RoleRepository.GetAll() on userRole.RoleId equals role.Id
                          where user.Id == userId
                          select new UserDetailsDTO
                          {
                              Id = user.Id,
                              UserName = user.UserName,
                              Email = user.Email,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              PhoneNumber = user.PhoneNumber,
                              RoleId = role.Id,
                              Role = role.Name
                          }).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates encoded email token with redirect uri.
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<EmailDTO> GetEmailTokenWithContentAsync(AppUser appUser)
        {
            try
            {
                //Invalidates any previous codes sent to the user
                await _unitOfWork.SaveChangesAsync();
                var result = await _userManager.UpdateSecurityStampAsync(appUser);
                Uri callBackUri = await SetClientURIWithEncodedToken(appUser, SharedConstants.URIParam);
                var emailMessage = new EmailDTO
                {
                    To = appUser.Email,
                    Subject = SharedConstants.SignUpSubject,
                    Body = @$"Your StreamHarbor account has been created. Click the link below to confirm your email address and finish the sign up process.This link will expire after 24 hours.<br/>
                              <a href={callBackUri}>Click Here</a>"
                };
                return emailMessage;
            }
            catch (Exception ex)
            {
      Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<EmailDTO> GetEmailTokenWithContentResetAsync(AppUser appUser)
        {
            try
            {
                //Invalidates any previous codes sent to the user
                await _unitOfWork.SaveChangesAsync();
                var result = await _userManager.UpdateSecurityStampAsync(appUser);
                Uri callBackUri = await SetClientURIWithEncodedTokenResetPassword(appUser, SharedConstants.URIParamReset);
                var emailMessage = new EmailDTO
                {
                    To = appUser.Email,
                    Subject = SharedConstants.ResetSubject,
                    Body = @$"Dear user, Click the link below to reset your password. This link will expire after 24 hours.<br/>
                              <a href={callBackUri}>Click Here</a>"
                };
                return emailMessage;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Encodes email token and generates a client URI.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public async Task<Uri> SetClientURIWithEncodedToken(AppUser appUser, string uriParam)
        {
            string token = await EncodeTokenAsync(appUser);
            string clientUri = _configuration["URL"]; 
            appUser.UserName = appUser.UserName ?? "0";
            string generatedUri = $"{clientUri}/{uriParam}/{appUser.Id}/{appUser.Email}/{token}";
            Uri callBackUri = new Uri(Uri.EscapeUriString(generatedUri));
            return callBackUri;
        }

        public async Task<Uri> SetClientURIWithEncodedTokenResetPassword(AppUser appUser, string uriParam)
        {
            string token = await EncodePasswordTokenAsync(appUser);
            string clientUri = _configuration["URL"];
            appUser.UserName = appUser.UserName ?? "0";
            string generatedUri = $"{clientUri}/{uriParam}/{appUser.Id}/{appUser.Email}/{token}";
            Uri callBackUri = new Uri(Uri.EscapeUriString(generatedUri));
            return callBackUri;
        }

        //Generate Token
        public async Task<string>GenerateJWTToken(AppUser appUser)
        {
            var user = await _userManager.FindByEmailAsync(appUser.Email);
            var claims = new[]
            {
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenAsString;
        }

        /// <summary>
        /// Generates token for email and encodes into base64.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<string> EncodeTokenAsync(AppUser appUser)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }

        /// <summary>
        /// Generates token for password reset and encodes into base64.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<string> EncodePasswordTokenAsync(AppUser appUser)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }


        /// <summary>
        /// Validates if provided email token is valid or not
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ValidateEmailTokenAsync(AppUser appUser, string token)
        {
            try
            {
                return await _userManager.ConfirmEmailAsync(appUser, DecodeToken(token));
            }
            catch (Exception ex)
            {
//Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Generates token for email and encodes into base64.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private string DecodeToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        #endregion GetMethods
    }
}