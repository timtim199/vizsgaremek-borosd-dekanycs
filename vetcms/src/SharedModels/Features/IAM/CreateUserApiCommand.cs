using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.IAM.Authorization;
using static vetcms.SharedModels.Features.IAM.CreateUserApiCommandValidator;

namespace vetcms.SharedModels.Features.IAM
{
    public record CreateUserApiCommand : AuthenticatedApiCommandBase<CreateUserApiCommandResponse>
    {
        /// <summary>
        /// A felhasználó e-mail címe.
        /// </summary>
        public string Email { get; init; }

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
            return Path.Join(ApiBaseUrl, "/api/v1/iam/create-user");
        }


        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }

        public override PermissionFlags[] GetRequiredPermissions()
        {
            return [PermissionFlags.CAN_CREATE_USER];
        }
    }

    public class CreateUserApiCommandValidator : AbstractValidator<CreateUserApiCommand>
    {
        public CreateUserApiCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.PhoneNumber).Length(11);
        }
    }


    public class CreateUserApiCommandResponse : ICommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public CreateUserApiCommandResponse()
        {    
        }

        public CreateUserApiCommandResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}
