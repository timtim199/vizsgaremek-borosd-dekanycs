using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.ResetPassword
{
    internal class ConfirmResetPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ConfirmResetPasswordApiCommand, ConfirmResetPasswordApiCommandResponse>
    {
        public async Task<ConfirmResetPasswordApiCommandResponse> Handle(ConfirmResetPasswordApiCommand request, CancellationToken cancellationToken)
        {
            if (userRepository.HasUserByEmail(request.Email))
            {
                User user = userRepository.GetByEmail(request.Email);
                if (ValidateVerificationCode(user, request.ConfirmationCode))
                {
                    await ExecutePasswordReset(user, request.NewPassword);
                    await InvalidateVerificationCode(user, request.ConfirmationCode);
                    return new ConfirmResetPasswordApiCommandResponse(true);
                }
                else
                {
                    return new ConfirmResetPasswordApiCommandResponse(false, "Hibás kód!");
                }
            }
            else
            {
                return new ConfirmResetPasswordApiCommandResponse(false, "Nincs ilyen felhasználó.");
            }
        }

        // TODO: 30 perc után lejár a kód
        private bool ValidateVerificationCode(User user, string code)
        {
            code = code.ToUpper();
            userRepository.LoadReferencedCollection(user, u => u.PasswordResets);
            return user.PasswordResets.Any(pr => pr.Code == code && !pr.Deleted);
        }

        private async Task ExecutePasswordReset(User user, string newPassword)
        {
            user.Password = PasswordUtility.CreateUserPassword(user, newPassword);
            await userRepository.UpdateAsync(user);
        }

        private async Task InvalidateVerificationCode(User user, string code)
        {
            PasswordReset passwordReset = user.PasswordResets.First(pr => pr.Code == code);
            passwordReset.Deleted = true;
            await userRepository.UpdateAsync(user);
        }
    }
}
