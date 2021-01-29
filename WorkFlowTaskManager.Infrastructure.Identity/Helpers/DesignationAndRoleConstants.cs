using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkFlowTaskManager.Infrastructure.Identity.Helpers
{
    public class DesignationRole
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }

        public DesignationRole(Guid id, string name)
        {
            Id = id;
            Name = name.ToLower();
            NormalizedName = name;
        }
    }

    public class DesignationAndRoleConstants
    {
        public const string Admin = "ADMIN";
        public const string SuperAdmin = "SUPERADMIN";
        public const string MarketingManager = "MARKETINGMANAGER";
        public const string MarketingAssociate = "MARKETINGASSOCIATE";
        public const string Reader = "READER";

        private static List<DesignationRole> _designationRole;

        static DesignationAndRoleConstants()
        {
            _designationRole = new List<DesignationRole>()
            {
                new DesignationRole(Guid.Parse("d0e2d200-79e8-4c5e-b624-bfe2f5104fcd"), Admin),
                new DesignationRole(Guid.Parse("d2e2d200-79e8-4c5e-b624-bfe2f5104fcd"), MarketingManager),
                new DesignationRole(Guid.Parse("d8e2d200-79e8-4c5e-b624-bfe2f5104fcd"), MarketingAssociate),
                new DesignationRole(Guid.Parse("c64e4197-7ae9-4405-a1b4-280577892644"), Reader),
            };
            if (_designationRole.GroupBy(dr => dr.Id).Any(drg => drg.Count() > 1))
            {
                throw new System.Exception("Duplicate primary keys (IDs) for designation and role.");
            }
        }

        public static List<DesignationRole> GetDefaultRoles() => _designationRole;
    }
}