using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Features.IAM.ResetPassword
{
    public class ConfirmResetPasswordClientCommand : IClientCommand<bool>
    {
        public ConfirmResetPasswordClientCommand(IDialogService dialogService) : base(dialogService)
        {
        }

        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }


    }
}
