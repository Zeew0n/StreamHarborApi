
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Application.DTO.User.Response;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.Infrastructure.Identity.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserAuthService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
            IJwtService jwtService, IUserService userService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _userService = userService;
        }

        #region AuthToken

        public async Task<AuthenticationResponseDTO> AuthenticateAsync(AuthenticationRequestDTO request)
        {
            try
            {
                var claimsIdentity = await GetClaimsIdentityAsync(request.UserName, request.Password);
                var jwtResponse = await _jwtService.GenerateJwt(claimsIdentity);
                //await GenerateRefreshToken(claimsIdentity);
                //jwtResponse.RefreshToken = claimsIdentity.RefreshToken.Token;
               // jwtResponse.RefreshTokenExpiry = claimsIdentity.RefreshToken.ExpiryDate;
                return jwtResponse;
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }


        public async Task<AuthenticationResponseDTO> AuthenticateTenantAsync(AuthenticationRequestDTO request)
        {
            try
            {
                var claimsIdentity = await GetClaimsIdentityAsync(request.UserName, request.Password);
                var jwtResponse = await _jwtService.GenerateJwt(claimsIdentity);
                //await GenerateRefreshToken(claimsIdentity);
                //jwtResponse.RefreshToken = claimsIdentity.RefreshToken.Token;
                // jwtResponse.RefreshTokenExpiry = claimsIdentity.RefreshToken.ExpiryDate;
                return jwtResponse;
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }


        private async Task GenerateRefreshToken(AppUserDTO userDetails)
        {
            var currentRefreshToken = userDetails.RefreshToken;
            if (currentRefreshToken != null)
                userDetails.RefreshToken = currentRefreshToken;
            else
            {
                Guid userId = userDetails.Id;
                var refreshToken = CreateRefreshToken(userId);
                //var refreshModel = refreshToken.MapToRefreshTokenResponseDTO();
                refreshToken.CreatedBy = userId;
                _unitOfWork.RefreshTokenRepository.Insert(refreshToken);
                await _unitOfWork.SaveChangesAsync();
                //userDetails.RefreshToken = refreshModel;
            }
        }

        private RefreshToken CreateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return RefreshToken.CreateRefreshToken(Convert.ToBase64String(randomNumber), userId);
            }
        }

        /// <summary>
        /// Verifies user credentials and returns claim list asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AppUserDTO> GetClaimsIdentityAsync(string userName, string password)
        {
            try
            {
                AppUser appUser = await VerifyEmailOrUserNameAsync(userName);
                if (appUser == null)
                    throw new Exception($"No Accounts Registered with {userName}.");

                if (await _userManager.IsEmailConfirmedAsync(appUser))
                {
                    var userDetails = await GetUserDetailsByUserId(appUser.Id);
                    if (userDetails == null)
                        throw new Exception($"You must be assigned a role before you login.");

                    //if (!userDetails.IsAdmin && userDetails.Permissions.Count == 0)
                    //    throw new Exception($"You must be assigned a permission before you login.");

                    return await VerifyUserNamePasswordAsync(password, appUser,userDetails);
                }

                throw new Exception("You must confirm your email before you log in.");
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Verifies username and password and returns claim list asynchronously.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userToVerify"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private async Task<AppUserDTO> VerifyUserNamePasswordAsync(string password, AppUser userToVerify, AppUserDTO userDetails)
        {
            if (await _userService.CheckPasswordAsync(userToVerify, password))
            {
                var claimDTO = new ClaimDTO
                {
                    Id = userToVerify.Id,
                    UserName = userDetails.UserName,
                   // FullName = userDetails.FullName,
                    Email = userToVerify.Email,
                   // RoleId = userDetails.RoleId,
                   // Role = userDetails.RoleName,
                    //IsAdmin = userDetails.IsAdmin,
                   // Permissions = JsonConvert.SerializeObject(userDetails.Permissions)
                };
                userDetails.ClaimsIdentity = await Task.FromResult(_jwtService.GenerateClaimsIdentity(claimDTO));
                return userDetails;
            }

            throw new Exception("Invalid email or password.");
        }

        private async Task<AppUserDTO> GetUserDetailsByUserId(Guid userId)
        {
            var userDetails = await (from user in _unitOfWork.UserRepository.GetAll()
                                     join userRole in _unitOfWork.UserRoleRepository.GetAll() on user.Id equals userRole.UserId
                                     join role in _unitOfWork.RoleRepository.GetAll() on userRole.RoleId equals role.Id
                                     //join refToken in _unitOfWork.RefreshTokenRepository.GetAll() on user.Id equals refToken.UserId into rt
                                     //from refreshToken in rt.DefaultIfEmpty()
                                     where user.Id == userId
                                     select new
                                     {
                                         user.Id,
                                         FullName = user.FirstName + " " + user.LastName,
                                         user.UserName,
                                         user.Email,
                                         user.IsAdmin,
                                         RoleId = role.Id,
                                         RoleName = role.Name,
                                         Permission = "",
                                         RefreshToken = ""
                                     }).OrderBy(t => t.Permission).ToListAsync();
            return userDetails.GroupBy(t => t.RoleId)
                 .Select(q =>
                 {
                     var refreshToken = q.Select(t => t.RefreshToken).FirstOrDefault();
                     return new AppUserDTO
                     {
                         Id = q.Select(t => t.Id).FirstOrDefault(),
                         FullName = q.Select(t => t.FullName).FirstOrDefault(),
                         UserName = q.Select(t => t.UserName).FirstOrDefault(),
                         Email = q.Select(t => t.Email).FirstOrDefault(),
                         IsAdmin = q.Select(t => t.IsAdmin).FirstOrDefault(),
                         RoleId = q.Key,
                         RoleName = q.Select(t => t.RoleName).FirstOrDefault(),
                         //Permissions = q.Where(t => t.Permission != null).Select(t => t.Permission).ToList(),
                         //RefreshToken = refreshToken?.MapToRefreshTokenResponseDTO()
                     };
                 }).FirstOrDefault();
        }

        /// <summary>
        /// Verifies provided user data is username or email and fetches data asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userToVerify"></param>
        /// <returns></returns>
        private async Task<AppUser> VerifyEmailOrUserNameAsync(string userName)
        {
            return IsEmail(userName) ? await _userService.FindByEmailAsync(userName) : await _userService.FindByNameAsync(userName);
        }

        /// <summary>
        /// Checks username or email.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool IsEmail(string userName) => new EmailAddressAttribute().IsValid(userName);

        #endregion AuthToken

        #region RefreshToken

        public async Task<AuthenticationResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var claimsIdentity = await GetClaimsIdentityAsync(refreshToken);
                var jwtResponse = await _jwtService.GenerateJwt(claimsIdentity);
                await UpdateRefreshToken(claimsIdentity);
                jwtResponse.RefreshToken = claimsIdentity.RefreshToken.Token;
                jwtResponse.RefreshTokenExpiry = claimsIdentity.RefreshToken.ExpiryDate;
                return jwtResponse;
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task UpdateRefreshToken(AppUserDTO userDetails)
        {
            Guid userId = userDetails.Id;
            var currentRefreshToken = await _unitOfWork.RefreshTokenRepository.Find(q => q.UserId == userId).SingleOrDefaultAsync();
            currentRefreshToken.UpdateToken(GenerateRefreshToken());
            currentRefreshToken.UpdatedBy = userId;
            //var refreshModel = currentRefreshToken.MapToRefreshTokenResponseDTO();
            _unitOfWork.RefreshTokenRepository.Update(currentRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            //userDetails.RefreshToken = refreshModel;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Get claim list asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AppUserDTO> GetClaimsIdentityAsync(string token)
        {
            try
            {
                var userDetails = await GetUserDetailsByRefreshToken(token);
                if (userDetails == null)
                    throw new Exception($"Token did not match any users.");

                if (!userDetails.IsAdmin && userDetails.Permissions.Count == 0)
                    throw new Exception($"You must be assigned a permission before you login.");

                var refreshToken = userDetails.RefreshToken;
                return await GenerateClaimsIdentity(userDetails);
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Returns claim list asynchronously.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userToVerify"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private async Task<AppUserDTO> GenerateClaimsIdentity(AppUserDTO userDetails)
        {
            var claimDTO = new ClaimDTO
            {
                Id = userDetails.Id,
                UserName = userDetails.UserName,
                FullName = userDetails.FullName,
                Email = userDetails.Email,
                RoleId = userDetails.RoleId,
                Role = userDetails.RoleName,
                IsAdmin = userDetails.IsAdmin,
                Permissions = JsonConvert.SerializeObject(userDetails.Permissions)
            };
            userDetails.ClaimsIdentity = await Task.FromResult(_jwtService.GenerateClaimsIdentity(claimDTO));
            return userDetails;
        }

        private async Task<AppUserDTO> GetUserDetailsByRefreshToken(string token)
        {
            var userDetails = await (from refreshToken in _unitOfWork.RefreshTokenRepository.GetAll()
                                     join user in _unitOfWork.UserRepository.GetAll() on refreshToken.UserId equals user.Id
                                     join userRole in _unitOfWork.UserRoleRepository.GetAll() on user.Id equals userRole.UserId
                                     join role in _unitOfWork.RoleRepository.GetAll() on userRole.RoleId equals role.Id
                                     join rolePer in _unitOfWork.RolePermissionMappingRepository.GetAll() on role.Id equals rolePer.RoleId into rp
                                     from rolePermission in rp.DefaultIfEmpty()
                                     join perm in _unitOfWork.RolePermissionRepository.GetAll() on rolePermission.PermissionId equals perm.Id into per
                                     from permission in per.DefaultIfEmpty()
                                     where refreshToken.Token == token
                                     select new
                                     {
                                         user.Id,
                                         FullName = user.FirstName + " " + user.LastName,
                                         user.UserName,
                                         user.Email,
                                         user.IsAdmin,
                                         RoleId = role.Id,
                                         RoleName = role.Name,
                                         Permission = permission == null ? null : permission.Slug,
                                         RefreshToken = refreshToken
                                     }).OrderBy(t => t.Permission).ToListAsync();
            return userDetails.GroupBy(t => t.RoleId)
                 .Select(q =>
                 {
                     var refreshToken = q.Select(t => t.RefreshToken).FirstOrDefault();
                     return new AppUserDTO
                     {
                         Id = q.Select(t => t.Id).FirstOrDefault(),
                         FullName = q.Select(t => t.FullName).FirstOrDefault(),
                         UserName = q.Select(t => t.UserName).FirstOrDefault(),
                         Email = q.Select(t => t.Email).FirstOrDefault(),
                         IsAdmin = q.Select(t => t.IsAdmin).FirstOrDefault(),
                         RoleId = q.Key,
                         RoleName = q.Select(t => t.RoleName).FirstOrDefault(),
                         Permissions = q.Where(t => t.Permission != null).Select(t => t.Permission).ToList(),
                         //RefreshToken = refreshToken.MapToRefreshTokenResponseDTO()
                     };
                 }).FirstOrDefault();
        }

        #endregion RefreshToken

        #region RevokeRefreshToken

        public async Task<bool> RevokeTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception($"Token is required.");

            var currentRefreshToken = await _unitOfWork.RefreshTokenRepository.Find(q => q.Token == token).SingleOrDefaultAsync();

            // return false if no user found with token
            if (currentRefreshToken == null)
                throw new Exception($"Token did not match any users.");

            _unitOfWork.RefreshTokenRepository.DeleteAsync(currentRefreshToken.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        #endregion RevokeRefreshToken
    }
}