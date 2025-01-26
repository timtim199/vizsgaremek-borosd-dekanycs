using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace vetcms.ServerApplication.Common.Abstractions
{
    internal interface IMailService
    {
        public Task SendPasswordResetEmailAsync(PasswordReset passwordReset);
        public Task SendModifyOtherUserEmailAsync(User user, string changedPassword);
    }
}
