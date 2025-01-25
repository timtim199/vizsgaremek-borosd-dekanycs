using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Features.IAM.UserList
{
    public class UserListClientQuery : IClientCommand<UserListClientQueryResponse>
    {
        public string SearchQuery { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
