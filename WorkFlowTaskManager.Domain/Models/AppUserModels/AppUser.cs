using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkFlowTaskManager.Domain.Models.AppUserModels
{
    public class AppUser : IdentityUser<Guid>, ISoftDeletableEntity
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        //[NotMapped]
        public string FullName { get => FirstName + " " + LastName; }

        [NotMapped]
        public string Password { get; set; }

        //Standard Columns
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string OrganizationName { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string SubDomain { get; set; }
        public Guid TenantId { get; set; }
    }
}
