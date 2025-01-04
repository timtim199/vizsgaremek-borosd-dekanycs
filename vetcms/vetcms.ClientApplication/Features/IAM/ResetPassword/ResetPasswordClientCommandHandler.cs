using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.IAM;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ClientApplication.Features.IAM.ResetPassword
{
    internal class ResetPasswordClientCommandHandler(IMediator mediator) : IRequestHandler<ResetPasswordClientCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordClientCommand request, CancellationToken cancellationToken)
        {
            BeginResetPasswordApiCommand beginResetPasswordApiCommand = new BeginResetPasswordApiCommand()
            {
                Email = request.Email
            };
            BeginResetPasswordApiCommandResponse response = await mediator.Send(beginResetPasswordApiCommand);
            if (response.Success)
            {
                return true;
            }
            else
            {
                request.DialogService.ShowError(response.Message, "Hiba");
                return false;
            }

        }

        internal class ResetPasswordApiCommandHandler : GenericApiCommandHandler<BeginResetPasswordApiCommand, BeginResetPasswordApiCommandResponse>
        {
            public ResetPasswordApiCommandHandler(HttpClient httpClient, AuthenticationManger credentialStore)
                : base(httpClient, credentialStore)
            {
            }
        }

    }

}
