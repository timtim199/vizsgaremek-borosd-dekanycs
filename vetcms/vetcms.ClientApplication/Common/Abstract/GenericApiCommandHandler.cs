using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using vetcms.ClientApplication.Common.Authentication;
using vetcms.ClientApplication.Common.Exceptions;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;

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

        public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",await _credentialStore.GetAccessToken()
                );
            var response = await DispatchRequest(request);
            return await ProcessResponse(response);
        }

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
                    throw new NotImplementedException($"Http method not found: {Enum.GetName(request.GetApiMethod())}");
            }
        }

        private async Task<HttpResponseMessage?> ProcessGet(TCommand command)
        {
            return await _httpClient.GetAsync(command.GetApiEndpoint());
        }

        private async Task<HttpResponseMessage?> ProcessPost(TCommand command)
        {
            return await _httpClient.PostAsJsonAsync(command.GetApiEndpoint(), command);
        }

        private async Task<HttpResponseMessage?> ProcessPatch(TCommand command)
        {
            return await _httpClient.PatchAsJsonAsync(command.GetApiEndpoint(), command);
        }

        private async Task<HttpResponseMessage?> ProcessPut(TCommand command)
        {
            return await _httpClient.PutAsJsonAsync(command.GetApiEndpoint(), command);
        }

        private async Task<HttpResponseMessage?> ProcessDelete(TCommand command)
        {
            return await _httpClient.DeleteAsync(command.GetApiEndpoint());
        }


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
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ProblemDetails? problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                if (problem != null)
                {
                    throw new ApiCommandExecutionException(problem);
                }
                throw new Exception("Szerveroldali hiba történt a kérés során");
            }
            else
            {
                throw new Exception("Ismeretlen hiba történt a kérés során");
            }
        }

        private async Task<TResult> ProcessResult(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            TResult? result = JsonSerializer.Deserialize<TResult>(content);
            if(result == null)
            {
                throw new Exception("Üres vagy helytelen Http válasz.");
            }
            return result;
        }
    }
}
