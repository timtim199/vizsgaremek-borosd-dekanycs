using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.IAM.Authorization;
using static vetcms.SharedModels.Features.IAM.DeleteUserApiCommand;

namespace vetcms.SharedModels.Features.IAM
{
    public record DeleteUserApiCommand : AuthenticatedApiCommandBase<DeleteUserApiCommandResponse>
    {

        public List<int> Ids { get; set; }

        public DeleteUserApiCommand()
        {
            
        }

        public DeleteUserApiCommand(int id)
        {
            Ids = new List<int> { id };
        }

        public DeleteUserApiCommand(List<int> ids)
        {
            Ids = ids;
        }

        public DeleteUserApiCommand(params int[] ids)
        {
            Ids = ids.ToList();
        }

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/user");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Delete;
        }

        public override PermissionFlags[] GetRequiredPermissions()
        {
            return [PermissionFlags.CAN_DELETE_USERS];
        }
    }

    public class DeleteUserApiCommandValidator : AbstractValidator<DeleteUserApiCommand>
    {
        public DeleteUserApiCommandValidator()
        {
            RuleFor(x => x.Ids).NotEmpty();
        }
    }

    public record DeleteUserApiCommandResponse : ICommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public DeleteUserApiCommandResponse()
        {

        }

        public DeleteUserApiCommandResponse(bool _success, string _message = "")
        {
            Success = _success;
            Message = _message;
        }
    }
}
