using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO.Tenant;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models.Tenant;

namespace WorkFlowTaskManager.Application.Services.TenantService
{


    public class TenantService : ITenantService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public TenantService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

        public async  Task<TenantDto> CreateAsync(TenantDto model)
        {
            try
            {
                TenantInformation tenantInfo = new TenantInformation
                {



                    OrganizationName = model.OrganizationName,
                    OrganizationEmail = model.OrganizationEmail,
                    SubDomain = model.SubDomain,
                };
                var tenat = _unitOfWork.TenantRepository.Insert(tenantInfo);
                await _unitOfWork.SaveChangesAsync();

                return  _mapper.Map<TenantInformation, TenantDto>(tenat);
            }
            catch (Exception ex)
            {
                //Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw ex;
            }


        }

        public async  Task<IEnumerable<TenantDto>> GetAllTenants()
        {
            try
            {

                var tenantList = await _unitOfWork.TenantRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<TenantInformation>, IEnumerable<TenantDto>>(tenantList);

                //return tenantList;
            }
            catch(Exception ex)
            {

                throw ex;
            }
        }
    }

    }

