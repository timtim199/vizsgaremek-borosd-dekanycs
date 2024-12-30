using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.ApiLogicExceptionHandling
{
    public enum ApiLogicExceptionCode
    {
        [Description("Nincs hiba")]
        NONE = 0,
        [Description("Bejelentkezés szükséges.")]
        INVALID_AUTHENTICATION = 401001,
        [Description("Nem rendelkezik megfelelő jogosultságokkal.")]
        INSUFFICIENT_PERMISSIONS = 403001
    }
}
