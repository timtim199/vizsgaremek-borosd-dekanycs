using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.Authentication;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.ClientApplication.Features.Authentication.LoginUser
{
    internal class LoginUserClientCommandHandler(IMediator mediator) : IRequestHandler<LoginUserClientCommand, bool>
    {
        public async Task<bool> Handle(LoginUserClientCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("mediatr működik");
            await mediator.Send(new LoginUserApiCommand());
            throw new NotImplementedException("teszt");
            return true;
        }
    }

    internal class LoginUserApiCommandHandler : GenericApiCommandHandler<LoginUserApiCommand, ICommandResult>
    {
        public LoginUserApiCommandHandler(HttpClient httpClient, CredentialStore credentialStore)
            :base(httpClient, credentialStore)
        {
        }
    }
}
