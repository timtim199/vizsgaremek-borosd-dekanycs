using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.ApiLogicExceptionHandling
{
    internal class CommonApiLogicException : Exception
    {
        public ApiLogicExceptionCode ExceptionCode { get; private set; }
        public string Message { get; set; }
        public CommonApiLogicException(ApiLogicExceptionCode code, string message = "")
        {
            ExceptionCode = code;
            Message = message;
        }
    }
}
