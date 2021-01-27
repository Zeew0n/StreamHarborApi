using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Tenant;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.WebAPI.Controllers;

namespace WorkFlowTM.WebAPI.Controllers.Tenant
{
    //[Route("api/tenant")]

    [Route("api/tenant")]
    public class TenantController : BaseController
    {

        private readonly IUserAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;


        public TenantController(IUserAuthService authService, ITenantService tenantService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
            _tenantService = tenantService;

        }



        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> CreateTenant([FromBody] TenantDto tenantDto)
        {
            try
            {

                var result = await _tenantService.CreateAsync(tenantDto);
                if (result!=null)
                {
                        return Ok();
                    

                }

          
                return BadRequest(HandleActionResult($"Tenant Creation Failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("getall")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetAllTenants()
        {

            try
            {

                var tenantlist = await _tenantService.GetAllTenants();
                if(tenantlist != null)
                {
                    return Ok(tenantlist);
                }
                return BadRequest(HandleActionResult($"Tenants Not Found", StatusCodes.Status400BadRequest));

            }
            catch(Exception ex)
            {

                throw ex;
            }

        }

    }
}
