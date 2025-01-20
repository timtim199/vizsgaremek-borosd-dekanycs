using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.IAM.Commands.AuthenticationStatus;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.ClientApplication.Common.IAM.Commands.CheckPermissionQuery
{
    public class CheckPermissionQuery : IClientCommand<bool>
    {
        public PermissionFlags[] RequestedPermissions { get; set; }

        public CheckPermissionQuery(params PermissionFlags[] permissions)
            : base()
        {
            RequestedPermissions = permissions;
        }
    }
}
