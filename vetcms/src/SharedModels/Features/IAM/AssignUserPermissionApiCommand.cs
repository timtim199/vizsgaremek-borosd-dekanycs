using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.SharedModels.Features.IAM
{
    public record AssignUserPermissionApiCommand : AuthenticatedApiCommandBase<AssignUserPermissionApiCommandResponse>
    {
        public int Id { get; init; }
        public string PermissionSet { get; set; }

        public EntityPermissions GetPermissions()
        {
            return new EntityPermissions(PermissionSet);
        }

        public override string GetApiEndpoint()
        {
            return Path.Join(ApiBaseUrl, "/api/v1/iam/assign-permission");
        }

        public override HttpMethodEnum GetApiMethod()
        {
            return HttpMethodEnum.Put;
        }

        public override PermissionFlags[] GetRequiredPermissions()
        {
            return [PermissionFlags.CAN_ASSIGN_PERMISSIONS];
        }
    }

    public class AssignUserPermissionApiCommandValidator : AbstractValidator<AssignUserPermissionApiCommand>
    {
        public AssignUserPermissionApiCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public record AssignUserPermissionApiCommandResponse : ICommandResult
    {
        public AssignUserPermissionApiCommandResponse()
        {
            
        }

        public AssignUserPermissionApiCommandResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
