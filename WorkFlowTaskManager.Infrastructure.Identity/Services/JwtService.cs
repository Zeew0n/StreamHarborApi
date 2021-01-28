using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.User.Response;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Infrastructure.Identity.Helpers;

namespace WorkFlowTaskManager.Infrastructure.Identity.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        private readonly IRoleServices _roleServices;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;


        public JwtService(UserManager<AppUser> userManager,IOptions<JwtIssuerOptions> jwtOptions, IRoleServices roleServices, IUserService userService)
        {
            _jwtOptions = jwtOptions.Value;
            _roleServices = roleServices;
            _userManager = userManager;
            _userService = userService;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        /// <summary>
        /// Generates claim with email, userid, role and roleaccess.
        /// </summary>
        /// <param name="claimDTO"></param>
        /// <returns></returns>
        public ClaimsIdentity GenerateClaimsIdentity(ClaimDTO claimDTO)
        {
            try
            {
                return new ClaimsIdentity(new GenericIdentity(claimDTO.Email, "Token"), AddToClaimList(claimDTO));
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Serializes encoded token with expiry to json string.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponseDTO> GenerateJwt(AppUserDTO userDetails)
        {
            try
            {
                string authToken = await GenerateEncodedToken(userDetails.ClaimsIdentity);
                var appuser = await _userManager.FindByIdAsync(userDetails.Id.ToString());
                List<string> roles = (List<string>)await _userManager.GetRolesAsync(appuser);
                string tenantId = await _userService.GetTenantId(appuser);
                return new AuthenticationResponseDTO
                {
                    JWToken = authToken,
                    RoleName = roles[0],
                    TenantId = tenantId,
                    ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                //Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Creates a base64 encoded token with provided user claims.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        private async Task<string> GenerateEncodedToken(ClaimsIdentity identity)
        {
            string email = identity.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
            //string isAdmin = identity.Claims.Single(c => c.Type == AuthConstants.IsAdmin).Value;
            var claimList = new List<Claim>() {
                     identity.FindFirst(AuthConstants.JwtId),
                     new Claim(JwtRegisteredClaimNames.Sub, email),
                     new Claim(JwtRegisteredClaimNames.Email, email),
                     identity.FindFirst(ClaimTypes.Name),
                     identity.FindFirst(AuthConstants.UserName),
                     //new Claim(AuthConstants.IsAdmin, isAdmin, ClaimValueTypes.Boolean),
                     //identity.FindFirst(AuthConstants.RoleId),
                     //identity.FindFirst(ClaimTypes.Role),
                     //identity.FindFirst(AuthConstants.Permissions),
                     new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                     new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                };
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claimList,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        /// <summary>
        /// Adds userid, role and roleaccess to claim list.
        /// </summary>
        /// <param name="claimDTO"></param>
        /// <returns></returns>
        private IEnumerable<Claim> AddToClaimList(ClaimDTO claimDTO)
        {
            yield return new Claim(AuthConstants.JwtId, claimDTO.Id.ToString());
            //yield return new Claim(AuthConstants.IsAdmin, claimDTO.IsAdmin.ToString(), ClaimValueTypes.Boolean);
            yield return new Claim(ClaimTypes.Email, claimDTO.Email);
            //yield return new Claim(ClaimTypes.Name, claimDTO.FullName);
            //yield return new Claim(AuthConstants.UserName, claimDTO.UserName);
            //yield return new Claim(AuthConstants.RoleId, claimDTO.RoleId.ToString());
            //yield return new Claim(ClaimTypes.Role, claimDTO.Role);
            //yield return new Claim(AuthConstants.Permissions, claimDTO.Permissions);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        /// <summary>
        /// Throws exceptions for invalid JwtIssuerOptions.
        /// </summary>
        /// <param name="options"></param>
        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null) throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }
    }
}