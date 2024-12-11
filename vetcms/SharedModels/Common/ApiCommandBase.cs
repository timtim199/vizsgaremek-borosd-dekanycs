using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Common
{
    public abstract record ApiCommandBase<T> : IRequest<T>
        where T : ICommandResult
    {
        public abstract string GetApiEndpoint();
        public abstract HttpMethodEnum GetApiMethod();
    }
}
