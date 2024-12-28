using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common.Abstract;

namespace vetcms.SharedModels.Common.Behaviour
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ApiCommandBase<TResponse>
    where TResponse : ICommandResult, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Any())
                {
                    TResponse response = new TResponse();
                    response.Success = false;
                    response.Message = TrasformFailures(failures);
                    return response;
                    //throw new ValidationException(failures);
                }
            }

            return await next();
        }
        private string TrasformFailures(List<ValidationFailure> failures)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Mezők helyes kitöltése kötelező: ");
            failures.ForEach(f => stringBuilder.AppendLine(f.ToString()));
            return stringBuilder.ToString();
        }
 
    }

}
