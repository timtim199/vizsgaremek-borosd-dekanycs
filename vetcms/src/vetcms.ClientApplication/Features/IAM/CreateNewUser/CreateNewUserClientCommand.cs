using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.SharedModels.Common.Dto;

namespace vetcms.ClientApplication.Features.IAM.CreateNewUser
{
    public class CreateNewUserClientCommand : IClientCommand<bool>
    {
        public UserDto NewUserModel { get; set; }
    }
}
