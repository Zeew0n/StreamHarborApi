using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WorkFlowTaskManager.Application.DTO.Role;
using WorkFlowTaskManager.Application.DTO.User.Request;
using WorkFlowTaskManager.Domain.Models.AppUserModels;

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
        }
    }
}
