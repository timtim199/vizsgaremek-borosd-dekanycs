using MediatR;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.IAM;
using vetcms.SharedModels.Common.IAM.Authorization;
namespace vetcms.ServerApplication.Features.IAM.LoginUser
{
    internal class LoginUserCommandHandler(UserRepository userRepository) : IRequestHandler<LoginUserApiCommand, LoginUserApiCommandResponse>
    {

        public Task<LoginUserApiCommandResponse> Handle(LoginUserApiCommand request, CancellationToken cancellationToken)
        {
            // kapcsolódó logika

            User user = userRepository.GetByEmail(request.Email);
            if (user.GetPermissions().HasPermissionFlag(PermissionFlags.IAM_LOGIN))
            {
                PasswordUtility utility = new PasswordUtility();
                if(utility.VerifyPassword(request.Password, user.Password))
                {
                    
                }
            }

            return Task.FromResult(new LoginUserApiCommandResponse()
            {
                Success = false,
                Message = "Hibás bejelentkezési adatok."
            });

            throw new NotImplementedException();
        }
    }
}
