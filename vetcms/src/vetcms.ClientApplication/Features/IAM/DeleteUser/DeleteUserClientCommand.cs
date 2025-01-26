using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Features.IAM.DeleteUser
{
    public class DeleteUserClientCommand : IClientCommand<bool>
    {
        public List<int> UserIds { get; set; }
    }
}
