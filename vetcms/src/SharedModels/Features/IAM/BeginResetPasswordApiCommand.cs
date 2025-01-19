using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.SharedModels.Features.IAM
{
    /// <summary>
    /// Az API parancs, amely elindítja a jelszó visszaállítási folyamatot.
    /// </summary>
    public record BeginResetPasswordApiCommand : UnauthenticatedApiCommandBase<BeginResetPasswordApiCommandResponse>
    {
        /// <summary>
        /// A felhasználó e-mail címe.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/reset-password/begin");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    /// <summary>
    /// Az API parancs bemeneti validátorja.
    /// </summary>
    public class BeginResetPasswordApiCommandValidator : AbstractValidator<BeginResetPasswordApiCommand>
    {
        public BeginResetPasswordApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    /// <summary>
    /// Az API parancs válasza, amely elindítja a jelszó visszaállítási folyamatot.
    /// </summary>
    public record BeginResetPasswordApiCommandResponse : ICommandResult
    {
        /// <summary>
        /// A parancs sikerességét jelző tulajdonság.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A parancs üzenetét tartalmazó tulajdonság.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public BeginResetPasswordApiCommandResponse()
        {
            
        }

        public BeginResetPasswordApiCommandResponse(bool _success, string _message = "")
        {
            Success = _success;
            Message = _message; 
        }
    }
}
