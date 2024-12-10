using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common
{
    public abstract record ApiCommandBase<T> : IRequest<T>
    {
        public string? BearerToken { get; set; }
        public abstract string GetApiEndpoint();
        public abstract HttpMethod GetApiMethod();
    }
}
