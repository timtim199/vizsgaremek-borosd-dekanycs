using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;

//https://www.rapidtables.com/convert/number/base-converter.html
namespace vetcms.SharedModels.Common.IAM.Authorization
{
    public class EntityPermissions
    {
        private BigInteger permissionSet;

        public EntityPermissions()
        {
            permissionSet = BigInteger.Zero;
        }

        public EntityPermissions(string initialPermissions)
        {
            permissionSet = BigInteger.Parse(initialPermissions);
        }

        public EntityPermissions(BigInteger initialPermissions)
        {
            permissionSet = initialPermissions;
        }

        public bool HasPermissionFlag(PermissionFlags flag)
        {
            return HasFlagAtPosition((int)flag);
        }

        private bool HasFlagAtPosition(int position)
        {
            return (permissionSet & (BigInteger.One << position)) != 0;
        }

        public void ClearAllFlags()
        {
            permissionSet = BigInteger.Zero;
        }

        public void RemoveFlag(PermissionFlags flag)
        {
            RemoveFlagAtPosition((int)flag);
        }

        private void RemoveFlagAtPosition(int position)
        {
            permissionSet &= ~(BigInteger.One << position);
        }

        public void AddFlag(PermissionFlags flag)
        {
            AddFlagAtPosition((int)flag);
        }

        public void AddFlagAtPosition(int position)
        {
            permissionSet |= BigInteger.One << position;
        }

        public EntityPermissions MergePermissions(EntityPermissions other)
        {
            permissionSet |= other.permissionSet;
            return this;
        }

        public override string ToString()
        {
            return permissionSet.ToString();
        }
    }
}
