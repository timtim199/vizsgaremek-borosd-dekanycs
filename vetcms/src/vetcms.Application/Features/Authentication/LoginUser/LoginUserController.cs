using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Common.Abstractions.Api;
using vetcms.SharedModels.Features.Authentication;

namespace vetcms.Application.Features.Authentication.LoginUser
{
    public partial class Authentication : ApiControllerBase
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
