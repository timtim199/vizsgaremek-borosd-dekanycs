using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.SharedModels.Common.Abstract
{
    public interface ICommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
