using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ClientApplication.Common.Exceptions
{
    internal class ApiCommandExecutionUnknownException : Exception
    {
        public ProblemDetails Problem { get; set; }
        public ApiCommandExecutionUnknownException(ProblemDetails problemDetails)
        {
            Problem = problemDetails;
        }
    }
}
