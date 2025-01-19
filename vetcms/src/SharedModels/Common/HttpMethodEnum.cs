using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common
{
    /// <summary>
    /// Az API HTTP metódusokat reprezentáló felsorolás.
    /// </summary>
    public enum HttpMethodEnum
    {
        /// <summary>
        /// HTTP GET metódus.
        /// </summary>
        Get,
        /// <summary>
        /// HTTP POST metódus.
        /// </summary>
        Post,
        /// <summary>
        /// HTTP PUT metódus.
        /// </summary>
        Put,
        /// <summary>
        /// HTTP DELETE metódus.
        /// </summary>
        Delete,
        /// <summary>
        /// HTTP PATCH metódus.
        /// </summary>
        Patch,
    }
}
