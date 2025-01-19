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
    /// Az API parancs, amely a felhasználó regisztrációját kezeli.
    /// </summary>
    public record RegisterUserApiCommand : UnauthenticatedApiCommandBase<RegisterUserApiCommandResponse>
    {
        /// <summary>
        /// A felhasználó e-mail címe.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// A felhasználó jelszava.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// A felhasználó telefonszáma.
        /// </summary>
        public string PhoneNumber { get; init; }

        /// <summary>
        /// A felhasználó neve.
        /// </summary>
        public string Name { get; init; }


        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/register");
        }


        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    /// <summary>
    /// Az API parancs bemeneti validátora.
    /// </summary>
    public class RegisterUserApiCommandValidator : AbstractValidator<RegisterUserApiCommand>
    {
        public RegisterUserApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email mező nem maradhat üresen, és email formátumban kell lennie, pl.: kallapal@example.hu");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Jelszó mező nem lehet üres!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Név mező nem lehet üres!");
            RuleFor(x => x.PhoneNumber).Length(11).WithMessage("Helytelen telefonszám!");
        }
    }

    /// <summary>
    /// Az API parancs válasza, amely a felhasználó regisztrációját kezeli.
    /// </summary>
    public record RegisterUserApiCommandResponse : ICommandResult
    {
        public RegisterUserApiCommandResponse()
        {
            
        }

        public RegisterUserApiCommandResponse(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// A parancs sikerességét jelző tulajdonság.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A parancs üzenetét tartalmazó tulajdonság.
        /// </summary>
        public string Message { get; set; }
    }
}
