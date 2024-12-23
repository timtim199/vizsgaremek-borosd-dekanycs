using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.BusinessExpections
{
    public static class CommonBusinessExpectionCodeResolver
    {
        private static Dictionary<CommonBusinessExpectionCodes, string> resolver = new Dictionary<CommonBusinessExpectionCodes, string>()
        { 
            { CommonBusinessExpectionCodes.INVALID_BASIC_AUTH_CREDENTIALS, "Hibás felhasználónév vagy jelszó" }
        };
        public static string ResolveCode(this CommonBusinessExpectionCodes code)
        {
            return resolver[code];
        }
    }
}
