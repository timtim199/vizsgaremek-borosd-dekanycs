using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Api;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.ResetPassword
{
    public partial class IamController : ApiV1ControllerBase
    {
        [HttpPost("reset-password/begin")]
        public async Task<ICommandResult> BeginResetPassword(BeginResetPasswordApiCommand command)
        {
            command.Prepare(Request);
            return await Mediator.Send(command);
        }

        [HttpPost("reset-password/confirm")]
        public async Task<ICommandResult> ConfirmResetPassword(ConfirmResetPasswordApiCommand command)
        {
            command.Prepare(Request);
            return await Mediator.Send(command);
        }
    }
}
