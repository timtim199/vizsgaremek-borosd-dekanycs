using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common
{
    public abstract record UnauthenticatedApiCommandBase<T> : ApiCommandBase<T>
    {
    }
}
