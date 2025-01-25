using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.SharedModels.Common.Dto;

namespace vetcms.ClientApplication.Features.IAM.ModifyUser
{
    public class ModifyUserClientCommand : IClientCommand<bool>
    {
        public int UserId { get; set; }
        public UserDto ModifiedUserDto { get; set; }
    }
}
