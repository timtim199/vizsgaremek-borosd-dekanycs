using Microsoft.AspNetCore.Mvc;
using vetcms.Application.Common.Abstractions.Api;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.Application.Features.Authentication.LoginUser
{
    public partial class Authentication : ApiV1ControllerBase
    {
        public Authentication()
        {
            
        }

        [HttpPost("login")]
        public async Task<int> LoginUser(LoginUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
