using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.Authentication.LoginUser;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.ClientApplication.Common.Authentication.Commands.AuthenticationStatus
{
    internal class AuthenticatedStatusQueryHandler : IRequestHandler<AuthenticatedStatusQuery, AuthenticatedStatusResponseModel>
    {
        public async Task<AuthenticatedStatusResponseModel> Handle(AuthenticatedStatusQuery request, CancellationToken cancellationToken)
        {
            return new AuthenticatedStatusResponseModel(true);
        }
    }
}
