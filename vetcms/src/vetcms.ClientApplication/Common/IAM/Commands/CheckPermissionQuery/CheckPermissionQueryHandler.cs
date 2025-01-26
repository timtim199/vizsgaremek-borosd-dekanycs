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
            if (await authenticationManger.IsAuthenticated())
            {
                bool result = await authenticationManger.HasPermission(request.RequestedPermissions);
                Console.WriteLine("Permission query: " + string.Join(", ", request.RequestedPermissions.Select(p => p.ToString()))+ $"Result: {result}"); // TODO: Remove this line
                //return result;
                return true; // TODO: Remove this line

            }
            else
                return false;
        }
    }
}
