using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ClientApplication.Features.IAM.ResetPassword
{
    internal class ConfirmResetPasswordClientCommandHandler(IMediator mediator) : IRequestHandler<ConfirmResetPasswordClientCommand, bool>
    {
        /// <summary>
        /// Kezeli a jelszó visszaállításának megerősítésére vonatkozó kérést.
        /// </summary>
        /// <param name="request">A jelszó visszaállításának megerősítésére vonatkozó kérés.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Igaz, ha a jelszó visszaállítása sikeres, különben hamis.</returns>
        public async Task<bool> Handle(ConfirmResetPasswordClientCommand request, CancellationToken cancellationToken)
        {
            if(request.Password1 != request.Password2)
            {
                request.DialogService.ShowError("A jelszók nem egyeznek!", "Hiba");
                return false;
            }
            else
            {
                ConfirmResetPasswordApiCommand confirmResetPasswordApiCommand = new ConfirmResetPasswordApiCommand()
                {
                    Email = request.Email,
                    ConfirmationCode = request.VerificationCode,
                    NewPassword = request.Password1
                };
                ConfirmResetPasswordApiCommandResponse response = await mediator.Send(confirmResetPasswordApiCommand);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
