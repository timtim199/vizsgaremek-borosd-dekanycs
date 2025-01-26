using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.FirstTimeAuthenticateUser
{
    internal class FirstTimeAuthenticateUserCommandHandler(IUserRepository userRepository, IFirstTimeAuthenticationCodeRepository firstTimeAuthenticationCodeRepository)
        : IRequestHandler<FirstTimeAuthenticateUserApiCommand, FirstTimeAuthenticateUserApiCommandResponse>
    {
        public async Task<FirstTimeAuthenticateUserApiCommandResponse> Handle(FirstTimeAuthenticateUserApiCommand request, CancellationToken cancellationToken)
        {
            User user = firstTimeAuthenticationCodeRepository.GetUserByCode(request.AuthenticationCode);


            user.Password = PasswordUtility.CreateUserPassword(user, request.Password);
            user.OverwritePermissions(new EntityPermissions().AddFlag(PermissionFlags.CAN_LOGIN));
            await userRepository.UpdateAsync(user);

            await DeleteAuthCode(user);

            return await Task.FromResult(new FirstTimeAuthenticateUserApiCommandResponse(true));

        }

        private async Task DeleteAuthCode(User user)
        {
            var authCode = firstTimeAuthenticationCodeRepository.GetCodeByUser(user);
            if (authCode != null)
            {
                var firstTimeAuthCode = firstTimeAuthenticationCodeRepository.Where(c => c.Code == authCode).FirstOrDefault();
                if (firstTimeAuthCode != null)
                {
                    firstTimeAuthCode.Deleted = true;
                    await firstTimeAuthenticationCodeRepository.UpdateAsync(firstTimeAuthCode);
                }
            }

        }
    }
}

