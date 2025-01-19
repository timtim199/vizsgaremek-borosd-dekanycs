using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Common.Exceptions;
using vetcms.SharedModels.Common.ApiLogicExceptionHandling;
using Microsoft.AspNetCore.Components;
using System.Net;
using vetcms.ClientApplication.Common.IAM;
using Microsoft.AspNetCore.Http.Authentication;

namespace vetcms.ClientApplication.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IClientCommand<TResponse>
    {
        private readonly IDialogService dialogService;
        private readonly NavigationManager navigationManager;
        private readonly AuthenticationManger authenticationManger;
        public UnhandledExceptionBehaviour(IDialogService _dialogService, NavigationManager _navigationManager, AuthenticationManger _authenticationManger)
        {
            navigationManager = _navigationManager;
            dialogService = _dialogService;
            authenticationManger = _authenticationManger;
        }

        /// <summary>
        /// Kezeli a kérést és végrehajtja az érvényesítést.
        /// </summary>
        /// <param name="request">Az API kérés.</param>
        /// <param name="next">A következő kezelő a csővezetékben.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Az API válasz.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch(CommonApiLogicException ex)
            {
                return await HandleApiLogicException(request, ex);
            }
            catch(ApiCommandExecutionUnknownException ex)
            {
                request.DialogService.ShowError(ex.Problem.detail, $"Váratlan hiba történt: {ex.Problem.title}");
                return default;
            }
            catch (Exception ex)
            {
                request.DialogService.ShowError(ex.StackTrace, $"Váratlan hiba történt: {ex.GetType().FullName}: {ex.Message}");
                return default;
            }
        }
        
        /// <summary>
        /// Kezeli az API logikai kivételt.
        /// </summary>
        /// <param name="request">Az API kérés.</param>
        /// <param name="ex">Az API logikai kivétel.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<TResponse> HandleApiLogicException(TRequest request, CommonApiLogicException ex)
        {
            IDialogReference r = await request.DialogService.ShowErrorAsync(ex.GetExceptionCodeDescription(), "Hiba történt a kérés feldolgozása közben.");
            await r.Result;
            switch (ex.ExceptionCode)
            {
                case ApiLogicExceptionCode.INVALID_AUTHENTICATION:
                    await authenticationManger.ClearAuthenticationDetails();
                    navigationManager.NavigateTo("/iam/login?redirected=true");
                    break;
                case ApiLogicExceptionCode.INSUFFICIENT_PERMISSIONS:
                    navigationManager.NavigateTo("/");
                    break;
            }

            return default!;
        }

    }

}
