using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.IAM;
using vetcms.SharedModels.Features.IAM;
using Microsoft.Extensions.DependencyInjection;

namespace vetcms.ClientApplication.Features.IAM.ResetPassword
{
    internal class ResetPasswordClientCommandHandler(IMediator mediator) : IRequestHandler<ResetPasswordClientCommand, bool>
    {
        /// <summary>
        /// Kezeli a jelszó visszaállításának megkezdésére vonatkozó kérést.
        /// </summary>
        /// <param name="request">A jelszó visszaállításának megkezdésére vonatkozó kérés.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Igaz, ha a jelszó visszaállításának megkezdése sikeres, különben hamis.</returns>
        public async Task<bool> Handle(ResetPasswordClientCommand request, CancellationToken cancellationToken)
        {
            BeginResetPasswordApiCommand beginResetPasswordApiCommand = new BeginResetPasswordApiCommand()
            {
                Email = request.Email
            };
            BeginResetPasswordApiCommandResponse response = await mediator.Send(beginResetPasswordApiCommand);
            if (response.Success)
            {
                request.DialogService.ShowSuccess(response.Message, "Siker");
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
            public ResetPasswordApiCommandHandler(IServiceScopeFactory serviceScopeFactory)
                : base(serviceScopeFactory)
            {
            }
        }

    }

}
