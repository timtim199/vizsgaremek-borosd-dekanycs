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
    internal class LoginUserClientCommandHandler(IMediator mediator, AuthenticationManger authenticationManger) : IRequestHandler<LoginUserClientCommand, bool>
    {
        /// <summary>
        /// Kezeli a bejelentkezési kérést.
        /// </summary>
        /// <param name="request">A bejelentkezési kérés.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Igaz, ha a bejelentkezés sikeres, különben hamis.</returns>
        public async Task<bool> Handle(LoginUserClientCommand request, CancellationToken cancellationToken)
        {
            LoginUserApiCommand loginUserApiCommand = new LoginUserApiCommand()
            {
                Email = request.Username,
                Password = request.Password
            };
            LoginUserApiCommandResponse response =  await mediator.Send(loginUserApiCommand);
            if(response.Success)
            {
                await authenticationManger.SaveAccessToken(response.AccessToken);
                await authenticationManger.SavePermissionSet(response.PermissionSet);
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
