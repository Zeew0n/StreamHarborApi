using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO;
using WorkFlowTaskManager.Domain.Models;

namespace WorkFlowTaskManager.Application.Services.InternalCompanyService
{
    public interface IInternalCompanyService
    {
        Task<bool> CreateAsync(InternalCompany company);

        Task<bool> DeleteAsync(Guid id);

        Task<List<InternalCompanyDTO>>GetAllAsync(Guid? id = null);

        Task<InternalCompanyDTO> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(InternalCompany company);
    }
}
