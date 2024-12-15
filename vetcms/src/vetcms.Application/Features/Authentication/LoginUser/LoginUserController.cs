﻿using Microsoft.AspNetCore.Mvc;
using vetcms.ServerApplication.Common;
using vetcms.ServerApplication.Common.Abstractions.Api;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.ServerApplication.Features.Authentication.LoginUser
{
    public partial class Authentication : ApiV1ControllerBase
    {
        public Authentication()
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