using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.SharedModels.Common.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string VisibleName { get; set; }
        public string? Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PermissionSet { get; private set; }

        public EntityPermissions GetPermissions()
        {
            if(string.IsNullOrEmpty(PermissionSet))
            {
                return new EntityPermissions();
            }
            return new EntityPermissions(PermissionSet);
        }

        public UserDto OverwritePermissions(EntityPermissions newPermissions)
        {
            PermissionSet = newPermissions.ToString();
            return this;
        }
    }
}
