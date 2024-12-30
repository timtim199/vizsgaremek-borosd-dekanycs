using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.ApiLogicExceptionHandling
{
    public enum ApiLogicExceptionCode
    {
        NONE = 0,
        INVALID_AUTHENTICATION = 401001,
        INSUFFICIENT_PERMISSIONS = 403001
    }
}
