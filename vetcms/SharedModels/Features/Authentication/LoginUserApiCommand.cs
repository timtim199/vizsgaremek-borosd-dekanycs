using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Features.Authentication
{
    public record LoginUserApiCommand : UnauthenticatedApiCommandBase<ICommandResult>
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public override string GetApiEndpoint()
        {
            return "https://webhook.site/29c4b4f4-6bcb-4501-a1fd-41a7b2e8c3c4";
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Get;
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
}
