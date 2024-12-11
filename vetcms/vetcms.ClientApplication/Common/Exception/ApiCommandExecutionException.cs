using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ClientApplication.Common.Exceptions
{
    internal class ApiCommandExecutionException : Exception
    {
        public ProblemDetails Problem { get; set; }
        public ApiCommandExecutionException(ProblemDetails problemDetails)
        {
            Problem = problemDetails;
        }
    }
}
