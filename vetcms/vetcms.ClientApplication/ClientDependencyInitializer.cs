using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using vetcms.SharedModels.Common.Behaviour;
using vetcms.ClientApplication.Common.Behaviours;
using vetcms.SharedModels.Features.Authentication;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Presistence;
using vetcms.ClientApplication.Common.Authentication;
using Blazored.LocalStorage;
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

            services.AddScoped<AuthenticationManger, AuthenticationManger>();

            return services;
        }

        public static IServiceCollection AddBrowserPresistence(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddScoped<IClientPresistenceDriver, BrowserPresistenceDriver>();
            return services;

        }
    }
}
