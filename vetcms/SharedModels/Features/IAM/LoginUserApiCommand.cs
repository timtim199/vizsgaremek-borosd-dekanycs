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
    public record LoginUserApiCommand() : UnauthenticatedApiCommandBase<LoginUserApiCommandResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "c4e75e88-a051-4f19-b7fe-1c8a492ff674");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    public class LoginUserCommandValidator : AbstractValidator<LoginUserApiCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public record LoginUserApiCommandResponse : ICommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string PermissionSet { get; set; }
        public string AccessToken { get; set; }
        public EntityPermissions GetPermissions()
        {
            return new EntityPermissions(PermissionSet);
        }
    }
}
