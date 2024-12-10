using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Features.Authentication.LoginUser
{
    public class LoginUserClientCommand : IClientCommand<bool>
    {
        public LoginUserClientCommand(IDialogService _dialogService) : base(_dialogService)
        {
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
