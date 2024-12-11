using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Common
{
    public abstract record AuthenticatedApiCommandBase<T> : ApiCommandBase<T>
        where T : ICommandResult
    {
        public string? BearerToken { get; set; }
    }
}
