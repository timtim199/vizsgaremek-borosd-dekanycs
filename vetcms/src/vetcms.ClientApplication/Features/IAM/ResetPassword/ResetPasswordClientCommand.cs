using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Features.IAM.ResetPassword
{
    public class ResetPasswordClientCommand : IClientCommand<bool>
    {
        public ResetPasswordClientCommand(IDialogService _dialogService) : base(_dialogService)
        {
        }

        public string Email { get; set; }
    }
}
