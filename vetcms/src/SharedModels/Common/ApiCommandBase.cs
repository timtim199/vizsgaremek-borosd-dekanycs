using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Common
{
    /// <summary>
    /// Az API parancsok alap osztálya.
    /// </summary>
    /// <typeparam name="T">A parancs eredményének típusa.</typeparam>
    public abstract record ApiCommandBase<T> : IRequest<T>
        where T : ICommandResult
    {
        internal const string ApiBaseUrl = "https://localhost:7129";

        /// <summary>
        /// Visszaadja a parancs API végpontját.
        /// </summary>
        /// <returns>Az API végpont.</returns>
        public abstract string GetApiEndpoint();

        /// <summary>
        /// Visszaadja az parancs HTTP metódusát.
        /// </summary>
        /// <returns>A HTTP metódus.</returns>
        public abstract HttpMethodEnum GetApiMethod();
    }
}
