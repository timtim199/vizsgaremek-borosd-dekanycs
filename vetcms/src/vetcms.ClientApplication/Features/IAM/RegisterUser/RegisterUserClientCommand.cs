using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Features.IAM.RegisterUser
{
    public class RegisterUserClientCommand : IClientCommand<bool>
    {
        public RegisterUserClientCommand(IDialogService _dialogService) : base(_dialogService)
        {
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
    }
}
