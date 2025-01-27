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
        //[Obslete("A field használata nem ajánlott, használja az BrowserPresentation/wwwroot/appsettings.json-t.")]
        public string ApiBaseUrl = "";

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
