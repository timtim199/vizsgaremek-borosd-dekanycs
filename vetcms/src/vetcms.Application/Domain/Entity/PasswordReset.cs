using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;

namespace vetcms.ServerApplication.Domain.Entity
{
    public class PasswordReset : AuditedEntity
    {
        public User User { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
