using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkFlowTaskManager.Application.Services.CurrentUserService;
using WorkFlowTaskManager.Domain.Models;
using WorkFlowTaskManager.Domain.Models.AppUserModels;
using WorkFlowTaskManager.Domain.Models.User;
using WorkFlowTaskManager.Infrastructure.Persistence.Extensions;

namespace WorkFlowTaskManager.Infrastructure.Persistance.Data
{

    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        private readonly ICurrentUserService _currentUserService;
        private const string IsDeletedColumnName = "IsDeleted";
        private const string DeletedByColumnName = "DeletedBy";
        private const string DeletedDateColumnName = "DeletedDate";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<bool>(IsDeletedColumnName).HasDefaultValue(false);
                    modelBuilder.Entity(entityType.ClrType).Property<Guid?>(DeletedByColumnName).IsRequired(false);
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime?>(DeletedDateColumnName).IsRequired(false);
                    modelBuilder.SetSoftDeleteFilter(entityType.ClrType, IsDeletedColumnName);
                }
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserSignUp>(ConfigureUserSignup);
            modelBuilder.Entity<RolePermissionMapping>(ConfigureRolePermissionMapping);
        }

        private void ConfigureRolePermissionMapping(EntityTypeBuilder<RolePermissionMapping> builder)
        {
            builder.HasKey(q => new { q.RoleId, q.PermissionId });
            builder.HasOne(q => q.Role)
                  .WithMany(q => q.RolePermissions)
                  .HasForeignKey(q => q.RoleId);
            builder.HasOne(q => q.Permission)
                   .WithMany(q => q.RolePermissions)
                   .HasForeignKey(q => q.PermissionId);
        }
        private void ConfigureUserSignup(EntityTypeBuilder<UserSignUp> builder)
        {
            builder.HasKey(q => q.Id);
        }


        //Entities (in A -> Z)
        public DbSet<UserSignUp> UserSignup { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        public DbSet<RolePermissionMapping> RolePermissionMapping { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            DateTime currentDateTime = DateTime.Now;
            var currentUser = _currentUserService.GetUser();
            bool isAuthenticateRequest = IsAuthenticatedRequest(currentUser);
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (isAuthenticateRequest)
                            entry.Entity.CreatedBy = currentUser;

                        entry.Entity.CreatedDate = currentDateTime;
                        break;

                    case EntityState.Modified:
                        if (isAuthenticateRequest)
                            entry.Entity.UpdatedBy = currentUser;

                        entry.Entity.UpdatedDate = currentDateTime;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDeletableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (isAuthenticateRequest)
                            entry.CurrentValues[DeletedByColumnName] = currentUser;

                        entry.CurrentValues[DeletedDateColumnName] = currentDateTime;
                        entry.CurrentValues[IsDeletedColumnName] = true;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            DateTime currentDateTime = DateTime.Now;
            var currentUser = _currentUserService.GetUser();
            bool isAuthenticateRequest = IsAuthenticatedRequest(currentUser);
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (isAuthenticateRequest)
                            entry.Entity.CreatedBy = currentUser;

                        entry.Entity.CreatedDate = currentDateTime;
                        break;

                    case EntityState.Modified:
                        if (isAuthenticateRequest)
                            entry.Entity.UpdatedBy = currentUser;

                        entry.Entity.UpdatedDate = currentDateTime;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDeletableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (isAuthenticateRequest)
                            entry.CurrentValues[DeletedByColumnName] = currentUser;

                        entry.CurrentValues[DeletedDateColumnName] = currentDateTime;
                        entry.CurrentValues[IsDeletedColumnName] = true;
                        break;
                }
            }

            return base.SaveChanges();
        }

        private bool IsAuthenticatedRequest(Guid currentUser) => currentUser != Guid.Empty;
    }
}
