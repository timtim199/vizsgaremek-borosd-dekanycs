using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.ServerApplication.Domain.Entity
{
    public class User : AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string VisibleName { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } 
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Password { get; set; } = "";
        public List<PasswordReset> PasswordResets { get; set; } = new();

        public string PermissionSet { get; private set; } = new EntityPermissions().AddFlag(PermissionFlags.CAN_LOGIN).ToString();
        public EntityPermissions GetPermissions()
        {
            return new EntityPermissions(PermissionSet);
        }

        public User OverwritePermissions(EntityPermissions newPermissions)
        {
            PermissionSet = newPermissions.ToString();
            return this;    
        }
    }
}
