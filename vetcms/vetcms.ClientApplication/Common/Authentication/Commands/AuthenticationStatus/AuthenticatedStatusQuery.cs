using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Common.Authentication.Commands.AuthenticationStatus
{
    public class AuthenticatedStatusQuery : IClientCommand<AuthenticatedStatusResponseModel>
    {
        public AuthenticatedStatusQuery(IDialogService dialogService)
            : base(dialogService)
        {

        }
    }
}
