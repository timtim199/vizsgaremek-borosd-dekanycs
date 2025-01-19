using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.SharedModels.Common
{
    public abstract record AuthenticatedApiCommandBase<T> : ApiCommandBase<T>
        where T : ICommandResult
    {
        protected AuthenticatedApiCommandBase()
        {
            
        }

        /// <summary>
        /// A Bearer token, amelyet az API hívásokhoz használnak.
        /// </summary>
        public string? BearerToken { get; set; }

        /// <summary>
        /// Visszaadja a szükséges jogosultságokat.
        /// </summary>
        /// <returns>A szükséges jogosultságok tömbje.</returns>
        public abstract PermissionFlags[] GetRequiredPermissions();
    }
}
