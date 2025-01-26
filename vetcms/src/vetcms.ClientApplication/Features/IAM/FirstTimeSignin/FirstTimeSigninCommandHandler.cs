using MediatR;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.DeleteUser;
using vetcms.ClientApplication.Features.IAM.LoginUser;

namespace vetcms.ClientApplication.Features.IAM.FirstTimeSignin
{
    internal class FirstTimeSigninCommandHandler(IDialogService dialogService) : IRequestHandler<FirstTimeSigninCommand, bool>
    {
        public async Task<bool> Handle(FirstTimeSigninCommand request, CancellationToken cancellationToken)
        {
            if(request.NewPassword != request.NewPasswordConfirmation)
            {
                await dialogService.ShowErrorAsync("Jelszó és a megerősítése sajnos nem egyezik.");
                return false;
            }
            await Task.Delay(1000);
            var reference = await dialogService.ShowSuccessAsync("Sikeres jelszó beállítás, kérjük jelentkezzen be.", "Siker!");
            var result = await reference.Result;
            return true;
        }
    }
}
