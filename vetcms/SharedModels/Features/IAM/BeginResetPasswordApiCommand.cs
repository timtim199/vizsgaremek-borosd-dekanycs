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
    public record BeginResetPasswordApiCommand : UnauthenticatedApiCommandBase<BeginResetPasswordApiCommandResponse>
    {
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

    public class BeginResetPasswordApiCommandValidator : AbstractValidator<BeginResetPasswordApiCommand>
    {
        public BeginResetPasswordApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public record BeginResetPasswordApiCommandResponse : ICommandResult
    {
        public bool Success { get; set; }
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
