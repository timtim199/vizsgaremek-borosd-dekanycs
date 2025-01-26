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
    public record ModifyOtherUserApiCommand : AuthenticatedApiCommandBase<ModifyOtherUserApiCommandResponse>
    {

        public int Id { get; set; }
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
        public string VisibleName { get; init; }
        //public string Address { get; set; }

        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public DateTime DateOfBirth { get; set; }

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/modify-other-user");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Post;
        }

        public override PermissionFlags[] GetRequiredPermissions()
        {
            return [PermissionFlags.CAN_MODIFY_OTHER_USER];
        }
    }

    public class ModifyOtherUserApiCommandValidator : AbstractValidator<ModifyOtherUserApiCommand>
    {
        public ModifyOtherUserApiCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.VisibleName).NotEmpty();
            RuleFor(x => x.PhoneNumber).Length(11);
        }
    }


    public record ModifyOtherUserApiCommandResponse : ICommandResult
    {
        public bool Success { get ; set ; }
        public string Message { get ; set ; }
        public ModifyOtherUserApiCommandResponse()
        {
            
        }

        public ModifyOtherUserApiCommandResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}
