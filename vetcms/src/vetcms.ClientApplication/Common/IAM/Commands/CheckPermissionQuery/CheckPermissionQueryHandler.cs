using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ClientApplication.Common.IAM.Commands.CheckPermissionQuery
{
    internal class CheckPermissionQueryHandler(AuthenticationManger authenticationManger)
        : IRequestHandler<CheckPermissionQuery, bool>
    {
        public async Task<bool> Handle(CheckPermissionQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Permission query: " + string.Join(", ", request.RequestedPermissions.Select(p => p.ToString()))); // TODO: Remove this line
            if (await authenticationManger.IsAuthenticated())
            {
                return await authenticationManger.HasPermission(request.RequestedPermissions);
            }
            else
                return false;
        }
    }
}
