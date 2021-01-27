using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.Interfaces;
using WorkFlowTaskManager.Domain.Models;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Domain.Models.Tenant;
using WorkFlowTaskManager.Infrastructure.Persistance.Data;

namespace WorkFlowTaskManager.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly ApplicationDbContext _dbContext;
        private IRepository<AppUser> _userRepository;
        private IRepository<AppRole> _roleRepository;
        private IRepository<IdentityUserRole<Guid>> _userRoleRepository;
        private IRepository<IdentityUserClaim<Guid>> _userClaimRepository;
        private IRepository<RolePermission> _rolePermissionRepository;
        private IRepository<RolePermissionMapping> _rolePermissionMappingRepository;

        private IRepository<RefreshToken> _refreshTokenRepository;

        private IRepository<InternalCompany> _internalCompanyRepository;

        private IRepository<TenantInformation> _tenantRepository;


        public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext ?? throw new ArgumentNullException("Context was not supplied");

        public IRepository<AppUser> UserRepository =>
            _userRepository ?? (_userRepository = new Repository<AppUser>(_dbContext));

        public IRepository<AppRole> RoleRepository =>
            _roleRepository ?? (_roleRepository = new Repository<AppRole>(_dbContext));

        public IRepository<IdentityUserRole<Guid>> UserRoleRepository =>
            _userRoleRepository ?? (_userRoleRepository = new Repository<IdentityUserRole<Guid>>(_dbContext));

        public IRepository<IdentityUserClaim<Guid>> UserClaimRepository =>
            _userClaimRepository ?? (_userClaimRepository = new Repository<IdentityUserClaim<Guid>>(_dbContext));

        public IRepository<InternalCompany> InternalCompanyRepository =>
_internalCompanyRepository ?? (_internalCompanyRepository = new Repository<InternalCompany>(_dbContext));


        public IRepository<TenantInformation>TenantRepository =>
_tenantRepository ?? (_tenantRepository = new Repository<TenantInformation>(_dbContext));

        public IRepository<RolePermission> RolePermissionRepository =>
            _rolePermissionRepository ?? (_rolePermissionRepository = new Repository<RolePermission>(_dbContext));

        public IRepository<RolePermissionMapping> RolePermissionMappingRepository =>
            _rolePermissionMappingRepository ?? (_rolePermissionMappingRepository = new Repository<RolePermissionMapping>(_dbContext));

        public IRepository<RefreshToken> RefreshTokenRepository =>
            _refreshTokenRepository ?? (_refreshTokenRepository = new Repository<RefreshToken>(_dbContext));

        public int SaveChanges() => _dbContext.SaveChanges();

        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();

        public void BeginTransaction()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(UnitOfWork).Name);

            if (_transaction == null)
                _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null)
                    _transaction.Commit();
            }
            finally
            {
                if (_transaction != null)
                    _transaction = null;
            }
        }

        public void RollBack()
        {
            try
            {
                if (_transaction != null)
                    _transaction.Rollback();
            }
            finally
            {
                if (_transaction != null)
                    _transaction = null;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}