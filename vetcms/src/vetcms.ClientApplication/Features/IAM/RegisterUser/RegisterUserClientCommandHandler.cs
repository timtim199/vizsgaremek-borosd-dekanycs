using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.IAM;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ClientApplication.Features.IAM.RegisterUser
{
    internal class RegisterUserClientCommandHandler(IMediator mediator) : IRequestHandler<RegisterUserClientCommand, bool>
    {
        /// <summary>
        /// Kezeli a regisztrációs kérést.
        /// </summary>
        /// <param name="request">A regisztrációs kérés.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Igaz, ha a regisztráció sikeres, különben hamis.</returns>
        public async Task<bool> Handle(RegisterUserClientCommand request, CancellationToken cancellationToken)
        {
            if(request.Password != request.Password2)
            {
                request.DialogService.ShowError("A jelszók nem egyeznek", "Hiba");
                return false;
            }
            else
            {
                RegisterUserApiCommand registerUserApiCommand = new RegisterUserApiCommand()
                {
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                    Name = request.Username
                };
                RegisterUserApiCommandResponse response = await mediator.Send(registerUserApiCommand);
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
        }

        internal class RegisterUserApiCommandHandler : GenericApiCommandHandler<RegisterUserApiCommand, RegisterUserApiCommandResponse>
        {
            public RegisterUserApiCommandHandler(HttpClient httpClient, AuthenticationManger credentialStore)
                : base(httpClient, credentialStore)
            {
            }
        }

    }
}
