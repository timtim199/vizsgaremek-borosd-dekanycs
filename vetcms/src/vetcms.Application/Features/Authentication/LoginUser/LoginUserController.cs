using Microsoft.AspNetCore.Mvc;
using vetcms.ServerApplication.Common.Abstractions.Api;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.ServerApplication.Features.Authentication.LoginUser
{
    public partial class Authentication : ApiV1ControllerBase
    {
        public Authentication()
        {

        }

        [HttpPost("login")]
        public async Task<int> LoginUser(LoginUserApiCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
