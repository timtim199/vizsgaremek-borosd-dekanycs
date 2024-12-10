using MediatR;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.ServerApplication.Features.Authentication.LoginUser
{
    internal class LoginUserCommandHandler(ApplicationDbContext context) : IRequestHandler<LoginUserApiCommand, int>
    {
        private readonly ApplicationDbContext _context = context;

        public Task<int> Handle(LoginUserApiCommand request, CancellationToken cancellationToken)
        {
            // kapcsolódó logika
            throw new NotImplementedException();
        }
    }
}
