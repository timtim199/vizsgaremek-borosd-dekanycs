using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Common.Behaviours
{
    public class UnhandledExpectionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IClientCommand<TResponse>
    {
        private readonly IDialogService dialogService;
        public UnhandledExpectionBehaviour(IDialogService _dialogService)
        {
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                request.DialogService.ShowError(ex.StackTrace, $"Váratlan hiba történt: {ex.GetType().FullName}: {ex.Message}");
                return default;
            }
        }
    }
}
