using AutoMapper;
using WorkFlowTaskManager.Application.DTO.Role;
//using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Application.Interfaces;

namespace WorkFlowTaskManager.Application.Services
{
    public class RoleServices : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> CreateAsync(RoleDto model)
        {
            try
            {  
                AppRole appRole = new AppRole
                {
                    Name = model.RoleName,
                    NormalizedName = model.RoleName.ToUpper()
                };
                var Role = _unitOfWork.RoleRepository.Insert(appRole);
                await _unitOfWork.SaveChangesAsync();

               
                return _mapper.Map<AppRole, RoleDto>(Role);
            }
            catch(Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
          
           
        }

       

        public async Task<bool> DeleteAsync(Guid Id)
        {
            try
            { 
                var model = new RoleDto()
                {
                    RoleId = Id
                };
                
                var query = await _unitOfWork.RoleRepository.Find(a => a.Id == Id).FirstOrDefaultAsync();
               
                if (query == null)
                {
                    Log.Error("Error: Couldn't find Role code  {id} id", Id);
                    throw new NullReferenceException($"Couldn't find Role code by {Id} id");
                }
                _unitOfWork.RoleRepository.Delete(query);
                await RolePremission(model);


                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;

            }
        }

        public async Task<List<RoleDto>> GetAllAsync(Guid? id = null)
        {
            try
            {
                var res = await _unitOfWork.RoleRepository.GetAllAsync();
                return (_mapper.Map<IEnumerable<AppRole>, IEnumerable<RoleDto>>(res)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<RoleDto> GetByIdAsync(Guid id)
        {
            try
            {
                var res = await _unitOfWork.RoleRepository.GetAll(x => x.Id == id).Select(x => new RoleDto
                {
                    RoleId = x.Id,
                    RoleName = x.Name


                }).FirstOrDefaultAsync();


                return res;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(RoleDto model)
        {
            try
            {
                var roles = await _unitOfWork.RoleRepository.Find(x => x.Id == model.RoleId).SingleOrDefaultAsync();

                roles.Name = model.RoleName ?? roles.Name;
                roles.NormalizedName = model.RoleName.ToUpper() ?? roles.Name.ToUpper();


                _unitOfWork.RoleRepository.Update(roles);

                await _unitOfWork.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        #region
        private async Task<bool>RolePremission(RoleDto model)
        {
            try 
            { 
           

                var roles = await _unitOfWork.RolePermissionMappingRepository.GetAllAsync(x => x.RoleId == model.RoleId);
                var rolePermissionMapping = new List<RolePermissionMapping>();
                if (model.Premission != null)
                {
                        foreach (var role in model.Premission)
                        {
                            var premission = new RolePermissionMapping();

                            premission.PermissionId = role;
                            premission.RoleId = model.RoleId;
                            rolePermissionMapping.Add(premission);

                         };
                    if (roles == null)
                    {
                        _unitOfWork.RolePermissionMappingRepository.InsertRange(roles);

                    }
                    else
                    {
                        _unitOfWork.RolePermissionMappingRepository.DeleteRange(roles);

                        _unitOfWork.RolePermissionMappingRepository.InsertRange(rolePermissionMapping);



                    };

                }
                else
                {
                    _unitOfWork.RolePermissionMappingRepository.DeleteRange(roles);
                }
                
              
               return true;
             }

               catch(Exception ex)
               {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
                }


        }

        #endregion
    }
}