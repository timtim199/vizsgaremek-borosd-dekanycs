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

        /// <summary>
        /// Ellenőrzi, hogy a megadott jogosultsági zászló be van-e állítva.
        /// </summary>
        /// <param name="flag">A jogosultsági zászló.</param>
        /// <returns>Igaz, ha a jogosultsági zászló be van állítva, különben hamis.</returns>
        public bool HasPermissionFlag(PermissionFlags flag)
        {
            return HasFlagAtPosition((int)flag);
        }

        /// <summary>
        /// Ellenőrzi, hogy a megadott jogosultsági zászlók be vannak-e állítva.
        /// </summary>
        /// <param name="flags">A jogosultsági zászlók tömbje.</param>
        /// <returns>Igaz, ha az összes jogosultsági zászló be van állítva, különben hamis.</returns>
        public bool HasPermissionFlag(params PermissionFlags[] flags)
        {
            foreach (PermissionFlags flag in flags) 
            {
                if(!HasFlagAtPosition((int)flag))
                    return false;
            }
            return true;
        }

        private bool HasFlagAtPosition(int position)
        {
            return (permissionSet & (BigInteger.One << position)) != 0;
        }

        /// <summary>
        /// Törli az összes jogosultsági zászlót.
        /// </summary>
        /// <returns>Az aktuális EntityPermissions objektum.</returns>
        public EntityPermissions ClearAllFlags()
        {
            permissionSet = BigInteger.Zero;
            return this;
        }

        /// <summary>
        /// Eltávolítja a megadott jogosultsági zászlót.
        /// </summary>
        /// <param name="flag">A jogosultsági zászló.</param>
        /// <returns>Az aktuális EntityPermissions objektum.</returns>
        public EntityPermissions RemoveFlag(PermissionFlags flag)
        {
            RemoveFlagAtPosition((int)flag);
            return this;
        }

        private EntityPermissions RemoveFlagAtPosition(int position)
        {
            permissionSet &= ~(BigInteger.One << position);
            return this;

        }

        /// <summary>
        /// Hozzáadja a megadott jogosultsági zászlót.
        /// </summary>
        /// <param name="flag">A jogosultsági zászló.</param>
        /// <returns>Az aktuális EntityPermissions objektum.</returns>
        public EntityPermissions AddFlag(PermissionFlags flag)
        {
            AddFlagAtPosition((int)flag);
            return this;
        }

        /// <summary>
        /// Hozzáadja a megadott jogosultsági zászlókat.
        /// </summary>
        /// <param name="flags">A jogosultsági zászlók tömbje.</param>
        /// <returns>Az aktuális EntityPermissions objektum.</returns>
        public EntityPermissions AddFlag(params PermissionFlags[] flags)
        {
            foreach(PermissionFlags permissionFlag in flags)
            {
                AddFlagAtPosition((int)permissionFlag);
            }
            return this;
        }

        private EntityPermissions AddFlagAtPosition(int position)
        {
            permissionSet |= BigInteger.One << position;
            return this;
        }

        /// <summary>
        /// Összevonja a megadott jogosultságokat az aktuális jogosultságokkal.
        /// </summary>
        /// <param name="other">A másik EntityPermissions objektum.</param>
        /// <returns>Az aktuális EntityPermissions objektum.</returns>
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
