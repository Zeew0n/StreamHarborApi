using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Application.Extensions;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Application.Services;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTM.WebAPI;

namespace WorkFlowTaskManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
      
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO user)
        {
            var appUser = new AppUser
            {
                FirstName = user.FirstName,
                Email = user.Email
              
            };
           await  _userService.CreateAsync(appUser, "");
            return Ok();

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            try
            {
                AppUser appUser = _mapper.Map<UserRegisterDTO, AppUser>(userRegisterDTO);
                Guid roleId = userRegisterDTO.RoleId;
                string role = await _roleService.GetRoleNameByIdAsync(roleId);
                var result = await _userService.CreateAsync(appUser, role);
                if (result.Succeeded)
                {
                    var roleResult = await _roleService.AddToRoleAsync(appUser, role);
                    if (roleResult.Succeeded)
                        return Ok();

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
        
        [HttpPut("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUpUser([FromBody] SignUpUserDTO userRegisterDTO)
        {
            try
            {
                Guard.Against.NullOrEmpty(userRegisterDTO.UserName, nameof(userRegisterDTO.UserName));
                Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                AppUser oldUser = await _userService.FindByIdAsync(userRegisterDTO.Id);
                var result = await _userService.SignUpUserAsync(oldUser, userRegisterDTO);
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


    }
}
