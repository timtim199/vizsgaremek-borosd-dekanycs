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
    public record RegisterUserApiCommand : UnauthenticatedApiCommandBase<RegisterUserApiCommandResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string PhoneNumber { get; init; }
        public string Name { get; init; }

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "c4e75e88-a051-4f19-b7fe-1c8a492ff674");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    public class RegisterUserApiCommandValidator : AbstractValidator<RegisterUserApiCommand>
    {
        public RegisterUserApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email mező nem maradhat üresen, és email formátumban kell lennie, pl.: kallapal@example.hu");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Jelszó mező nem lehet üres!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Név mező nem lehet üres!");
            RuleFor(x => x.PhoneNumber).Length(9).WithMessage("Helytelen telefonszám!");
        }
    }

    public record RegisterUserApiCommandResponse : ICommandResult
    {
        public RegisterUserApiCommandResponse()
        {
            
        }

        public RegisterUserApiCommandResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
