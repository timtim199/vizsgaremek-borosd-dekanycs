using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.DeleteUser;

namespace vetcms.ClientApplication.Features.IAM.CreateNewUser
{
    internal class CreateNewUserClientCommandHandler : IRequestHandler<CreateNewUserClientCommand, bool>
    {
        public async Task<bool> Handle(CreateNewUserClientCommand request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            return true;
        }
    }
}
