using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.LoginUser;

namespace vetcms.ClientApplication.Features.IAM.ModifyUser
{
    internal class ModifyUserClientCommandHandler : IRequestHandler<ModifyUserClientCommand, bool>
    {
        public async Task<bool> Handle(ModifyUserClientCommand request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            return true;
        }
    }
}
