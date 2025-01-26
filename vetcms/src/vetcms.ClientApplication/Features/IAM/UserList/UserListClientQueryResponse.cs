using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Dto;

namespace vetcms.ClientApplication.Features.IAM.UserList
{
    public class UserListClientQueryResponse
    {
        public List<UserDto> UserQueryResult { get; set; }
        public int ResultCount { get; set; }
    }
}
