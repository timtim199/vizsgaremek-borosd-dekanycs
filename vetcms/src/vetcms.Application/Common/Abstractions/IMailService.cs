using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Common.Abstractions
{
    internal interface IMailService
    {
        public Task SendPasswordResetEmailAsync(PasswordReset passwordReset);
    }
}
