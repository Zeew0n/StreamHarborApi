using AutoMapper;
using WorkFlowTaskManager.Application.DTO.Role;
using WorkFlowTaskManager.Application.DTO.Tenant;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Domain.Models.Tenant;

namespace WorkFlowTaskManager.Application.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AppRole, RoleDto>().ReverseMap();
            CreateMap<RoleDto, AppRole>().ReverseMap();
            CreateMap<CreateUserDTO, AppUser>().ReverseMap();
            CreateMap<AppUser, CreateUserDTO>().ReverseMap();
            CreateMap<TenantInformation, TenantDto>().ReverseMap();
        }
    }
}
