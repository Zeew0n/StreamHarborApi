using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO;
using WorkFlowTaskManager.Application.Services.InternalCompanyService;
using WorkFlowTaskManager.Domain.Models;

namespace WorkFlowTaskManager.WebAPI.Controllers.InternalCompanys
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalCompanyController : BaseController
    {

        private readonly IInternalCompanyService _internalService;
        private readonly IMapper _mapper;

        public InternalCompanyController(IInternalCompanyService internalService, IMapper mapper)
        {
            _internalService = internalService;
            _mapper = mapper;
        }

        [HttpPost("create")]
       //[Permission(Permission.AddInternalCompany)]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] InternalCompanyDTO companydto)
        {
            try
            {
                var task = _mapper.Map<InternalCompanyDTO, InternalCompany>(companydto);
                var campaign = await _internalService.CreateAsync(task);
                return Ok(campaign);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("listcompany")]
        [AllowAnonymous]
        //[Permission(Permission.ViewInternalCompany)]
        public async Task<IActionResult> Read()
        {
            try
            {
                var result = await _internalService.GetAllAsync();
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

        [HttpGet("detailcompany/{Id}")]
       // [AllowAnonymous]
       // [Permission(Permission.ViewInternalCompany)]
        public async Task<IActionResult> Read(Guid Id)
        {
            try
            {
                var result = await _internalService.GetByIdAsync(Id);
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

        [HttpPut("updatecompany")]
        //[Permission(Permission.UpdateInternalCompany)]
       // [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] InternalCompanyDTO actionType)
        {
            try
            {
                var actType = _mapper.Map<InternalCompanyDTO, InternalCompany>(actionType);
                var result = await _internalService.UpdateAsync(actType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("deletecompany/{Id}")]
        //[Permission(Permission.DeleteInternalCompany)]
        //[AllowAnonymous]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    await _internalService.DeleteAsync(Id);
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
