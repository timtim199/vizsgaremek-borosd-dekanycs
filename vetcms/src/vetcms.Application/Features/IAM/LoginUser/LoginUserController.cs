using Microsoft.AspNetCore.Mvc;
using vetcms.ServerApplication.Common;
using vetcms.ServerApplication.Common.Abstractions.Api;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.LoginUser
{
    public partial class IamController : ApiV1ControllerBase
    {
        public IamController()
        {

        }

        [HttpPost("login")]
        public async Task<ICommandResult> LoginUser(LoginUserApiCommand command)
        {
            command.Prepare(Request);
            return await Mediator.Send(command);
        }
    }
}
