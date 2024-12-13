using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using vetcms.SharedModels.Common.Behaviour;
using vetcms.ClientApplication.Common.Behaviours;
using vetcms.SharedModels.Features.Authentication;
namespace vetcms.ClientApplication
{
    public static class ClientDependencyInitializer
    {
        public static IServiceCollection AddClientApp(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ClientDependencyInitializer).Assembly);

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ClientDependencyInitializer).Assembly);
                options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                options.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
                options.RegisterGenericHandlers = true;
            });

            return services;
        }
    }
}
