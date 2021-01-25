using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.DTO;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Application.Services.InternalCompanyService;
using WorkFlowTaskManager.Domain.Models;


namespace MarketingEmailSystem.Application.Services.CompanyService
{
    public class InternalCompanyService : IInternalCompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InternalCompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(InternalCompany company)
        {
            try
            {
                var res = _unitOfWork.InternalCompanyRepository.Insert(company);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public  async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
               

                var query = await _unitOfWork.InternalCompanyRepository.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (query == null)
                {
                    Log.Error("Error: Couldn't find Role code  {id} id", id);
                    throw new NullReferenceException($"Couldn't find Role code by {id} id");
                }
                _unitOfWork.InternalCompanyRepository.Delete(query);
               
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;

            }
        }

        public async Task<List<InternalCompanyDTO>> GetAllAsync(Guid? id = null)
        {
            try
            {
                var res = await _unitOfWork.InternalCompanyRepository.GetAllAsync();
                return (_mapper.Map<IEnumerable<InternalCompany>, IEnumerable<InternalCompanyDTO>>(res)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<InternalCompanyDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var res = await _unitOfWork.InternalCompanyRepository.GetAll(x => x.Id == id).Select(x => new InternalCompanyDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UpdatedDate=Convert.ToDateTime(x.UpdatedDate).ToString(),
                    CreatedDate = Convert.ToDateTime(x.CreatedDate).ToString()

                }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> UpdateAsync(InternalCompany company)
        {
            try 
            { 
                var query = await _unitOfWork.InternalCompanyRepository.Find(x => x.Id == company.Id).SingleOrDefaultAsync();

               query.Name = company.Name ?? query.Name;
                //query.UpdatedDate = company.Name ?? query.Name;

                _unitOfWork.InternalCompanyRepository.Update(query);

        
                await _unitOfWork.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            };
        }
    }
}
