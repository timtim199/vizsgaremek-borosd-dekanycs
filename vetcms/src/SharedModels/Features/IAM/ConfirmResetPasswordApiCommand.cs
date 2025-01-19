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
    public record ConfirmResetPasswordApiCommand : UnauthenticatedApiCommandBase<ConfirmResetPasswordApiCommandResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
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

    public class ConfirmResetPasswordApiCommandValidator : AbstractValidator<ConfirmResetPasswordApiCommand>
    {
        public ConfirmResetPasswordApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.ConfirmationCode).NotEmpty().Length(6);
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }

    public record ConfirmResetPasswordApiCommandResponse : ICommandResult
    {
        public bool Success { get; set; }
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
