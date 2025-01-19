using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.IAM.Authorization
{
    /// <summary>
    /// Jogosultsági "zászlók" felsorolása.
    /// </summary>
    public enum PermissionFlags
    {
        /// <summary>
        /// Bejelentkezési jogosultság.
        /// </summary>
        CAN_LOGIN,
        /// <summary>
        /// Jogosultságok hozzárendelésének jogosultsága.
        /// </summary>
        CAN_ASSIGN_PERMISSIONS,
        /// <summary>
        /// Felhasználók listázásának jogosultsága.
        /// </summary>
        CAN_LIST_USERS,
    }
}
