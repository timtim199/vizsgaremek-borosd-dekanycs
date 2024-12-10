using MediatR;
using vetcms.Application.Infrastructure.Presistence;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.Application.Features.Authentication.LoginUser
{
    internal class LoginUserCommandHandler(ApplicationDbContext context) : IRequestHandler<LoginUserCommand, int>
    {
        private readonly ApplicationDbContext _context = context;

        public Task<int> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // kapcsolódó logika
            throw new NotImplementedException();
        }
    }
}
