using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Common
{
    /// <summary>
    /// Az autentikáció nélküli API parancsok alap osztálya.
    /// </summary>
    /// <typeparam name="T">A parancs eredményének típusa.</typeparam>
    public abstract record UnauthenticatedApiCommandBase<T> : ApiCommandBase<T>
        where T : ICommandResult
    {
    }
}
