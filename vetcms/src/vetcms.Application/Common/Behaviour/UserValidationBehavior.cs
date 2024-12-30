using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.IAM;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.ApiLogicExceptionHandling;

namespace vetcms.ServerApplication.Common.Behaviour
{
    public class UserValidationBehavior<TRequest, TResponse>(AuthenticationCommon authenticationCommon) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : AuthenticatedApiCommandBase<TResponse>
        where TResponse : ICommandResult
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!request.BearerToken.IsNullOrEmpty() && await authenticationCommon.ValidateToken(request.BearerToken))
            {
                return await next();
            }
            else
            {
                throw new CommonApiLogicException(ApiLogicExceptionCode.INVALID_AUTHENTICATION, "Hibás bearer token.");
            }
        }
    }
}
