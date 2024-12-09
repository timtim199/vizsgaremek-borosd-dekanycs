using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Infrastructure.Presistence;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.Application.Features.Authentication.LoginUser
{
    internal class LoginUserCommandHandler(ApplicationDbContext context) : IRequestHandler<LoginUserCommand, int>
    {
        private readonly ApplicationDbContext _context = context;

        public Task<int> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
