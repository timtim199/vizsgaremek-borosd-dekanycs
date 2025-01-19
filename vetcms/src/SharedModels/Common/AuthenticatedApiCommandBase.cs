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
        public string? BearerToken { get; set; }
        public abstract PermissionFlags[] GetRequiredPermissions();
    }
}
