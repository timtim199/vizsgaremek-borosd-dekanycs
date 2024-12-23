using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.BusinessExpections;

namespace vetcms.ServerApplication.Common.Exceptions
{
    public class CommonBusinessLogicException : Exception
    {
        public int status { get; set; }
        public CommonBusinessLogicException(int _status)
            :base()
        {
            status = _status;
        }

        public CommonBusinessLogicException(CommonBusinessExpectionCodes expectionCode)
        {
            status = (int)expectionCode;
        }
    }
}
