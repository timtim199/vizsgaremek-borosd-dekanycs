using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Common.Abstractions.Api;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.Application.Features.Authentication.RegisterUser
{
    public partial class Authentication : ApiV1ControllerBase
    {
        [HttpPost("register")]
        public async Task<int> RegisterUser(LoginUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
