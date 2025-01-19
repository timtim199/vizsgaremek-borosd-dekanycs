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
    /// Az API parancs, amely megerősíti a jelszó visszaállítását.
    /// </summary>
    public record ConfirmResetPasswordApiCommand : UnauthenticatedApiCommandBase<ConfirmResetPasswordApiCommandResponse>
    {
        /// <summary>
        /// A felhasználó e-mail címe.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// A megerősítő kód.
        /// </summary>
        public string ConfirmationCode { get; set; } = string.Empty;

        /// <summary>
        /// Az új jelszó.
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/reset-password/confirm");
        }


        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    /// <summary>
    /// Az API parancs bemeneti validátora.
    /// </summary>
    public class ConfirmResetPasswordApiCommandValidator : AbstractValidator<ConfirmResetPasswordApiCommand>
    {
        public ConfirmResetPasswordApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.ConfirmationCode).NotEmpty().Length(6);
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }

    /// <summary>
    /// Az API parancs válasza, amely megerősíti a jelszó visszaállítását.
    /// </summary>
    public record ConfirmResetPasswordApiCommandResponse : ICommandResult
    {
        /// <summary>
        /// A parancs sikerességét jelző tulajdonság.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A parancs üzenetét tartalmazó tulajdonság.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public ConfirmResetPasswordApiCommandResponse()
        {

        }

        public ConfirmResetPasswordApiCommandResponse(bool _success, string _message = "")
        {
            Success = _success;
            Message = _message;
        }
    }
}
