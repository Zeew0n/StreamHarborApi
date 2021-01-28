using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Email;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Application.Extensions;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Application.Services;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

namespace WorkFlowTaskManager.WebAPI.Controllers.User
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserAuthService _authService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IRoleServices _roleServices;
        private readonly IMailService _emailService;
        private readonly IMapper _mapper;
        //private readonly IConfiguration _configuration;

        public UserController(IUserAuthService authService, IUserService userService,
            IRoleService roleService, IRoleServices roleServices, IMapper mapper,IMailService emailService)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
            _roleServices = roleServices;
            _emailService = emailService;
        }

        #region Registration

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequestDTO request)
        {
            try
            {
                var result = await _authService.AuthenticateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult($"Username or Password is Invalid!", StatusCodes.Status400BadRequest));
            }
        }

        #region Commands

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] CreateUserDTO userRegisterDTO)
        {
            try
            {
                AppUser appUser = await _userService.FindByEmailAsync(userRegisterDTO.UserName);

                if (appUser == null)
                {
                    return BadRequest(HandleActionResult($"Email Not Found!", StatusCodes.Status400BadRequest));
                }

                Guid appuserid = Guid.Parse(appUser.Id.ToString());
                AppUser appnewUser = await _userService.FindByIdAsync(appuserid);
                if (appnewUser != null)
                {
                    var emailBody = await _userService.GetEmailTokenWithContentResetAsync(appnewUser);
                    var emailCommand = new EmailDTO
                    {
                        To = emailBody.To,
                        Body = emailBody.Body,
                        Subject = emailBody.Subject
                    };
                    await _emailService.SendEmailAsync(emailCommand);
                    return Ok();
                }

                return BadRequest(HandleActionResult($"Email Not Found!", StatusCodes.Status400BadRequest));
            }

            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePasswordReset([FromBody] UpdateUserDTO userRegisterDTO)
        {
            try
            {
                Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                AppUser User = await _userService.FindByIdAsync(userRegisterDTO.Id);
                var result = await _userService.ResetPasswordAsync(User,userRegisterDTO.token,userRegisterDTO.Password);
                if (result.Succeeded)
                {
                    return Ok();

                }
                return BadRequest(HandleActionResult($"Password Reset failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }



        [HttpGet]
        [Route("confirmUser")]
        public async Task<IActionResult> ConfirmUser(Guid userId, string token)
        {
            try
                    {
               // Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                AppUser User = await _userService.FindByIdAsync(userId);
                var result = await _userService.ConfirmUserAsync(User, token);
                if (result)
                {
                    return Ok();

                }
                return BadRequest(HandleActionResult($"Confirm User Failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }



        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userRegisterDTO)
        {
            try
            {
                Guid roleId = userRegisterDTO.RoleId;
                Guard.Against.NullOrEmpty(userRegisterDTO.FirstName, nameof(userRegisterDTO.FirstName));
                Guard.Against.NullOrEmpty(userRegisterDTO.LastName, nameof(userRegisterDTO.LastName));
                Guard.Against.InvalidEmail(userRegisterDTO.Email, nameof(userRegisterDTO.Email));
                Guard.Against.InvalidEmail(userRegisterDTO.ConfirmEmail, nameof(userRegisterDTO.ConfirmEmail));
                Guard.Against.InvalidCompare(userRegisterDTO.Email, userRegisterDTO.ConfirmEmail, nameof(userRegisterDTO.Email), nameof(userRegisterDTO.ConfirmEmail));
                //Guard.Against.NullOrEmpty(roleId, nameof(roleId));
                AppUser appUser = _mapper.Map<CreateUserDTO, AppUser>(userRegisterDTO);
                Guard.Against.InvalidPhone(userRegisterDTO.PhoneNumber);
                string role = await _roleServices.GetRoleNameByIdAsync(roleId);
               //default role removed for test purpose

                var result = await _userService.CreateAsync(appUser, role);
                if (result.Succeeded)
                {

                    var id1 = appUser.Id;
                    var user = await _userService.FindByIdAsync(appUser.Id);

                    var roleResult = await _roleServices.AddToRoleAsync(user,role );
                    if (roleResult.Succeeded)
                    {
                        var emailBody = await _userService.GetEmailTokenWithContentAsync(user);
                        var emailCommand = new EmailDTO
                        {
                            To = emailBody.To,
                            Body = emailBody.Body,
                            Subject = emailBody.Subject
                        };
                        await _emailService.SendEmailAsync(emailCommand);
                        return Ok(user);
                    }

                    return BadRequest(HandleActionResult($"User registration failed.", StatusCodes.Status400BadRequest));
                }

                string identityErrors = string.Empty;
                foreach (var item in result.Errors)
                {
                    identityErrors = string.Concat(identityErrors, item.Code, ": ", item.Description);
                }
                return BadRequest(HandleActionResult($"User registration failed. { identityErrors }", StatusCodes.Status400BadRequest));
            }
            
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO userRegisterDTO)
        {
            try
            {

                Guard.Against.NullOrEmpty(userRegisterDTO.FirstName, nameof(userRegisterDTO.FirstName));
                Guard.Against.NullOrEmpty(userRegisterDTO.LastName, nameof(userRegisterDTO.LastName));
                //Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                AppUser oldUser = await _userService.FindByIdAsync(userRegisterDTO.Id);
                var result = await _userService.UpdateUserAsync(oldUser, userRegisterDTO);
                if (result.Succeeded)
                {
                    return Ok();


                }
                return BadRequest(HandleActionResult($"User update failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("delete/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                Guid userIdd = Guid.Parse(userId);
                AppUser oldUser = await _userService.FindByIdAsync(userIdd);
                bool result = await _userService.DeleteUserAsync(oldUser, userIdd);
                if (result)
                {
                    return NoContent();
                }

                return BadRequest(HandleActionResult($"User delete failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPost("signup")]
        //[AllowAnonymous]
        //[Authorize]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> SignUpUser([FromBody] SignUpUserDTO userRegisterDTO)
        {
            try
            {
                Guard.Against.NullOrEmpty(userRegisterDTO.UserName, nameof(userRegisterDTO.UserName));
                Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                var result = await _userService.SignUpUserAsync(userRegisterDTO);
                if (result.Succeeded)
                    return Ok();

                string identityErrors = string.Empty;
                foreach (var item in result.Errors)
                {
                    identityErrors = string.Concat(identityErrors, item.Code, ": ", item.Description);
                }
                return BadRequest(HandleActionResult($"User registration failed. { identityErrors }", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] CreateUserDTO userRegisterDTO)
        {
            try
            {
                var user= await _userService.FindByNameAsync(userRegisterDTO.UserName);
                var result = await _userService.LoginAsync(user,userRegisterDTO.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }


        #endregion Commands

        #region Queries

        [HttpGet("listallusers")]
        //[Authorize]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUsers()
       {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        /// <summary>
        /// Verify email token.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("verifyEmailToken")]
        public async Task<IActionResult> VerifyEmailToken(Guid userId, string token)
        {
            try
            {
                AppUser appUser = await _userService.FindByIdAsync(userId);
                //token = token.Replace(" ", "%2b");
                var tokenResult = await _userService.ValidateEmailTokenAsync(appUser);
                if (tokenResult)
                    return Ok();

                return BadRequest(HandleActionResult("Invalid token", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult($"Token validation failed. { ex.Message}", StatusCodes.Status400BadRequest));
            }
        }


        [HttpGet("get/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserDetailById(Guid userId)
        {
            try
            {
                var result = await _userService.GetUserDetailById(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        #endregion Queries
    }
}

#endregion Registration