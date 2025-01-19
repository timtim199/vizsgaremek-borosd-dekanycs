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
        internal const string ApiBaseUrl = "https://localhost:7129";

        /// <summary>
        /// Visszaadja az API végpontot.
        /// </summary>
        /// <returns>Az API végpont.</returns>
        public abstract string GetApiEndpoint();

        /// <summary>
        /// Visszaadja az API metódust.
        /// </summary>
        /// <returns>Az API metódus.</returns>
        public abstract HttpMethodEnum GetApiMethod();
    }
}
