using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.LoginUser;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ClientApplication.Common.IAM.Commands.AuthenticationStatus
{
    internal class AuthenticatedStatusQueryHandler : IRequestHandler<AuthenticatedStatusQuery, AuthenticatedStatusResponseModel>
    {
        public async Task<AuthenticatedStatusResponseModel> Handle(AuthenticatedStatusQuery request, CancellationToken cancellationToken)
        {
            return new AuthenticatedStatusResponseModel(true);
        }
    }
}
