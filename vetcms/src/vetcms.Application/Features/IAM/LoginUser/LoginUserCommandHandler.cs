using MediatR;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.IAM;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.ServerApplication.Common.IAM;
namespace vetcms.ServerApplication.Features.IAM.LoginUser
{
    internal class LoginUserCommandHandler(UserRepository userRepository, AuthenticationCommon authenticationCommon) : IRequestHandler<LoginUserApiCommand, LoginUserApiCommandResponse>
    {

        public Task<LoginUserApiCommandResponse> Handle(LoginUserApiCommand request, CancellationToken cancellationToken)
        {
            // kapcsolódó logika
            User user;

            try
            {
                user = userRepository.GetByEmail(request.Email);
            }
            catch (InvalidOperationException ex) 
            {
                return Task.FromResult(new LoginUserApiCommandResponse()
                {
                    Success = false,
                    Message = "Nem létező felhasználó"
                });
            }

            if (user.GetPermissions().HasPermissionFlag(PermissionFlags.CAN_LOGIN))
            {
                if(PasswordUtility.VerifyPassword(request.Password, user.Password))
                {
                    return Task.FromResult(new LoginUserApiCommandResponse()
                    {
                        Success = true,
                        PermissionSet = user.GetPermissions().ToString(),
                        AccessToken = authenticationCommon.GenerateAccessToken(user)
                    });
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
