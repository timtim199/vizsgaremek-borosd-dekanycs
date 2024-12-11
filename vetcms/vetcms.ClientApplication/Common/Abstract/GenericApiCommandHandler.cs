using MediatR;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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
        public GenericApiCommandHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
        {
            return await DispatchRequest(request);
        }

        private async Task<TResult> DispatchRequest(TCommand request)
        {
            switch (request.GetApiMethod())
            {
                case HttpMethodEnum.Get:
                    return await ProcessGet(request);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<TResult> ProcessGet(TCommand command)
        {
            var response = await _httpClient.GetAsync(command.GetApiEndpoint());
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new Exception(content);
            }
            else if(response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ProblemDetails? problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                if(problem != null)
                {
                    throw new ApiCommandExecutionException(problem);
                }
                throw new Exception("Ismeretlen hiba történt a kérés során");
            }
            return default;
        }
    }
}
