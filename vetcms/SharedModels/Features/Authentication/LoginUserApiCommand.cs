using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;

namespace vetcms.SharedModels.Features.Authentication
{
    public record LoginUserApiCommand : UnauthenticatedApiCommandBase<int>
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public override string GetApiEndpoint()
        {
            throw new NotImplementedException();
        }

        public override HttpMethod GetApiMethod()
        {
            throw new NotImplementedException();
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
