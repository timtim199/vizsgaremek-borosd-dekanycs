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
    /// <summary>
    /// Érvényesítési viselkedés osztály, amely az API parancsok érvényesítését kezeli.
    /// </summary>
    /// <typeparam name="TRequest">Az API kérés típusa.</typeparam>
    /// <typeparam name="TResponse">Az API válasz típusa.</typeparam>
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ApiCommandBase<TResponse>
    where TResponse : ICommandResult, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Létrehoz egy új példányt a ValidationBehaviour osztályból.
        /// </summary>
        /// <param name="validators">Az érvényesítők gyűjteménye.</param>
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Kezeli a kérést és végrehajtja az érvényesítést.
        /// </summary>
        /// <param name="request">Az API kérés.</param>
        /// <param name="next">A következő kezelő a pipeline-ban.</param>
        /// <param name="cancellationToken">A lemondási token.</param>
        /// <returns>Az API válasz.</returns>
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

        /// <summary>
        /// Átalakítja az érvényesítési hibákat szöveges üzenetté.
        /// </summary>
        /// <param name="failures">Az érvényesítési hibák listája.</param>
        /// <returns>Az érvényesítési hibák szöveges üzenete.</returns>
        private string TrasformFailures(List<ValidationFailure> failures)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("A mezőket helyesen kell kitölteni: ");
            failures.ForEach(f => stringBuilder.AppendLine(f.ToString()));
            return stringBuilder.ToString();
        }
 
    }

}
