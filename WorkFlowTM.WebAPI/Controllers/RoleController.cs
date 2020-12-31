using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Role;
using WorkFlowTaskManager.Application.Services;
//using WorkFlowTaskManager.Application.Interfaces;

namespace WorkFlowTaskManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _role;
        private readonly IMapper _mapper;

        public RoleController(IRoleService role, IMapper mapper)
        {
            _role = role;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        //[AllowAnonymous]
       // [Permission(Permission.AddRole)]
        public async Task<IActionResult> Create([FromBody] RoleDto role)
        {
            var query = await _role.CreateAsync(role);
            return Ok(query);
        }

        [HttpGet("Listrole")]
        //[Permission(Permission.ViewRole)]
        public async Task<IActionResult> Read()
        {
            try
            {
                var result = await _role.GetAllAsync();
                if (!result.Any())
                {
                    return NotFound(HandleActionResult("No data available", StatusCodes.Status404NotFound));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("Role/{Id}")]
        //[Permission(Permission.ViewRole)]
        public async Task<IActionResult> Read(Guid Id)
        {
            try
            {
                var result = await _role.GetByIdAsync(Id);
                if (result == null)
                {
                    return NotFound(HandleActionResult("No data available", StatusCodes.Status404NotFound));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("UpdateRole")]
       // [Permission(Permission.UpdateRole)]
       // [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] RoleDto actionType)
        {
            try
            {

               
                var result = await _role.UpdateAsync(actionType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("deleterole/{Id}")]
       // [Permission(Permission.DeleteRole)]
        //[AllowAnonymous]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    await _role.DeleteAsync(Id);
                    return Ok();
                }
                else
                {
                    return BadRequest(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }
    }
}