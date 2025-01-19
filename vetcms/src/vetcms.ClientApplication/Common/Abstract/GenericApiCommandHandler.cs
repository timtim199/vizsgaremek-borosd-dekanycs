using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using vetcms.ClientApplication.Common.Exceptions;
using vetcms.ClientApplication.Common.IAM;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Common.ApiLogicExceptionHandling;

namespace vetcms.ClientApplication.Common.Abstract
{
    internal class GenericApiCommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ApiCommandBase<TResult>
    where TResult : ICommandResult
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationManger _credentialStore;
        public GenericApiCommandHandler(HttpClient httpClient, AuthenticationManger credentialStore)
        {
            _httpClient = httpClient;
            _credentialStore = credentialStore;

        }

        /// <summary>
        /// Kezeli a kérést és végrehajtja az érvényesítést.
        /// </summary>
        /// <param name="request">Az API kérés.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Az API válasz.</returns>
        public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            if(await _credentialStore.HasAccessToken())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", await _credentialStore.GetAccessToken()
                );
            }
            
            var response = await DispatchRequest(request);
            return await ProcessResponse(response);
        }

        /// <summary>
        /// Elküldi a kérést az API végpontra.
        /// </summary>
        /// <param name="request">Az API kérés.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<HttpResponseMessage?> DispatchRequest(TCommand request)
        {
            switch (request.GetApiMethod())
            {
                case HttpMethodEnum.Get:
                    return await ProcessGet(request);
                case HttpMethodEnum.Post:
                    return await ProcessPost(request);
                case HttpMethodEnum.Patch:
                    return await ProcessPatch(request);
                case HttpMethodEnum.Put:
                    return await ProcessPut(request);
                case HttpMethodEnum.Delete:
                    return await ProcessDelete(request);
                default:
                    throw new NotImplementedException($"Http method not implemented in request dispatcher: {Enum.GetName(request.GetApiMethod())}");
            }
        }

        /// <summary>
        /// Feldolgozza az API választ.
        /// </summary>
        /// <param name="response">Az API válasz.</param>
        /// <returns>Az API válasz eredménye.</returns>
        private async Task<TResult> ProcessResponse(HttpResponseMessage? response)
        {
            if(response == null)
            {
                throw new Exception("A szerver nem válaszolt a kérésre");
            }
            if (response.IsSuccessStatusCode)
            {
                return await ProcessResult(response);
            }
            else
            {
                await HandleFailedRequest(response);
                return default!;
            }
        }

        /// <summary>
        /// Kezeli a sikertelen API kérést.
        /// </summary>
        /// <param name="response">Az API válasz.</param>
        private async Task HandleFailedRequest(HttpResponseMessage? response)
        {
            ProblemDetails? problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            if (problem != null)
            {
                if (problem.type == typeof(CommonApiLogicException).ToString())
                {
                    throw new CommonApiLogicException((ApiLogicExceptionCode)problem.status, problem.detail);
                }
                else
                {
                    throw new ApiCommandExecutionUnknownException(problem);
                }
            }
            throw new Exception("Szerver oldali hiba történt a kérés során");
        }

        /// <summary>
        /// Feldolgozza az API válasz eredményét.
        /// </summary>
        /// <param name="response">Az API válasz.</param>
        /// <returns>Az API válasz eredménye.</returns>
        private async Task<TResult> ProcessResult(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            TResult? result = JsonSerializer.Deserialize<TResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            if(result == null)
            {
                throw new Exception("Üres vagy érvénytelen Http válasz.");
            }
            return result;
        }

        /// <summary>
        /// Feldolgozza a GET kérést.
        /// </summary>
        /// <param name="command">Az API kérés.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<HttpResponseMessage?> ProcessGet(TCommand command)
            => await _httpClient.GetAsync(command.GetApiEndpoint());

        /// <summary>
        /// Feldolgozza a POST kérést.
        /// </summary>
        /// <param name="command">Az API kérés.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<HttpResponseMessage?> ProcessPost(TCommand command)
            => await _httpClient.PostAsJsonAsync(command.GetApiEndpoint(), command);

        /// <summary>
        /// Feldolgozza a PATCH kérést.
        /// </summary>
        /// <param name="command">Az API kérés.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<HttpResponseMessage?> ProcessPatch(TCommand command)
            => await _httpClient.PatchAsJsonAsync(command.GetApiEndpoint(), command);

        /// <summary>
        /// Feldolgozza a PUT kérést.
        /// </summary>
        /// <param name="command">Az API kérés.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<HttpResponseMessage?> ProcessPut(TCommand command)
            => await _httpClient.PutAsJsonAsync(command.GetApiEndpoint(), command);

        /// <summary>
        /// Feldolgozza a DELETE kérést.
        /// </summary>
        /// <param name="command">Az API kérés.</param>
        /// <returns>Az API válasz.</returns>
        private async Task<HttpResponseMessage?> ProcessDelete(TCommand command)
            => await _httpClient.DeleteAsync(command.GetApiEndpoint());
    }
}
