using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.ClientApplication.Features.Authentication.LoginUser
{
    internal class LoginUserClientCommandHandler() : IRequestHandler<LoginUserClientCommand, bool>
    {
        public async Task<bool> Handle(LoginUserClientCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("mediatr működik");
            throw new NotImplementedException("teszt");
            return true;
        }
    }
}
