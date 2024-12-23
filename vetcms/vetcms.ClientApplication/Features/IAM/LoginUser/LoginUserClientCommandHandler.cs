using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.IAM;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ClientApplication.Features.IAM.LoginUser
{
    internal class LoginUserClientCommandHandler(IMediator mediator) : IRequestHandler<LoginUserClientCommand, bool>
    {
        public async Task<bool> Handle(LoginUserClientCommand request, CancellationToken cancellationToken)
        {
            LoginUserApiCommandResponse response =  await mediator.Send(new LoginUserApiCommand());
            if(response.Success)
            {
                return true;
            }
            else
            {
                request.DialogService.ShowError(response.Message, "Hiba!");
                return false;
            }
        }
    }

    internal class LoginUserApiCommandHandler : GenericApiCommandHandler<LoginUserApiCommand, LoginUserApiCommandResponse>
    {
        public LoginUserApiCommandHandler(HttpClient httpClient, AuthenticationManger credentialStore)
            : base(httpClient, credentialStore)
        {
        }
    }
}
