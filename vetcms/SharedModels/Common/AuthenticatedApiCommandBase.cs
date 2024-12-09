using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common
{
    public abstract record AuthenticatedApiCommandBase<T> : ApiCommandBase<T>
    {
        public string? BearerToken { get; set; }
    }
}
