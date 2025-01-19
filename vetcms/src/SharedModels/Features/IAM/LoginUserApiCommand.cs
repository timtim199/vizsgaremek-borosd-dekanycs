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
    /// Az API parancs, amely a felhasználó bejelentkezését kezeli.
    /// </summary>
    public record LoginUserApiCommand() : UnauthenticatedApiCommandBase<LoginUserApiCommandResponse>
    {
        /// <summary>
        /// A felhasználó e-mail címe.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// A felhasználó jelszava.
        /// </summary>
        public string Password { get; init; }

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/login");
        }


        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }
    }

    /// <summary>
    /// Az API parancs bemeneti validátora.
    /// </summary>
    public class LoginUserCommandValidator : AbstractValidator<LoginUserApiCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    /// <summary>
    /// Az API parancs válasza, amely a felhasználó bejelentkezését kezeli.
    /// </summary>
    public record LoginUserApiCommandResponse : ICommandResult
    {
        /// <summary>
        /// A parancs sikerességét jelző tulajdonság.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A parancs üzenetét tartalmazó tulajdonság.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A felhasználó jogosultságainak halmaza.
        /// </summary>
        public string PermissionSet { get; set; }

        /// <summary>
        /// A hozzáférési token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Visszaadja a jogosultságokat.
        /// </summary>
        /// <returns>Az EntityPermissions objektum.</returns>
        public EntityPermissions GetPermissions()
        {
            return new EntityPermissions(PermissionSet);
        }
    }
}
