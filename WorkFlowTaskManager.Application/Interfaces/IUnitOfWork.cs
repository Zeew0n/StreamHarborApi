using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using WorkFlowTaskManager.Domain.Models;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Domain.Models.Tenant;

namespace WorkFlowTaskManager.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppUser> UserRepository { get; }
        IRepository<AppRole> RoleRepository { get; }

        IRepository<TenantInformation> TenantRepository { get; }
        IRepository<IdentityUserRole<Guid>> UserRoleRepository { get; }
        IRepository<IdentityUserClaim<Guid>> UserClaimRepository { get; }
        IRepository<RolePermission> RolePermissionRepository { get; }
        IRepository<RolePermissionMapping> RolePermissionMappingRepository { get; }
        IRepository<RefreshToken> RefreshTokenRepository { get; }
        IRepository<InternalCompany> InternalCompanyRepository { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void BeginTransaction();

        void Commit();

        void RollBack();
    }
}