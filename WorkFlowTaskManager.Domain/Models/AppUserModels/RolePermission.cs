using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowTaskManager.Domain.Models
{
    public class RolePermission
    {
        public Guid Id { get; private set; }

        [MaxLength(100)]
        public string Name { get; private set; }

        [MaxLength(250)]
        public string Slug { get; private set; }

        [MaxLength(100)]
        public string Icon { get; private set; }

        public string Group { get; private set; }
        public string SubGroup { get; private set; }
        public ICollection<RolePermissionMapping> RolePermissions { get; set; }

        //For ef
        private RolePermission() { }

        public RolePermission(
            Guid id,
            string name,
            string slug,
            string group,
            string subGroup = "")
        {
            Id = id;
            Name = name;
            Slug = slug;
            Group = group;
            SubGroup = subGroup;
        }
    }
}