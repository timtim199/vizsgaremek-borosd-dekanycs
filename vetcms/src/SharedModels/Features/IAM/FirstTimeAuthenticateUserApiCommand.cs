using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Features.IAM
{
    public record FirstTimeAuthenticateUserApiCommand : UnauthenticatedApiCommandBase<FirstTimeAuthenticateUserApiCommandResponse>
    {
        public string AuthenticationCode { get; set; }
        public string Password { get; set; }
        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/user-initial-auth");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    public class FirtTimeAuthenticateUserApiCommandValidator : AbstractValidator<RegisterUserApiCommand>
    {
        public FirtTimeAuthenticateUserApiCommandValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
        }
    }


    public record FirstTimeAuthenticateUserApiCommandResponse : ICommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public FirstTimeAuthenticateUserApiCommandResponse()
        {
        }

        public FirstTimeAuthenticateUserApiCommandResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}
