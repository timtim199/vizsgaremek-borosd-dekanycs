using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using vetcms.SharedModels.Common.Behaviour;
using vetcms.ClientApplication.Common.Behaviours;
using vetcms.SharedModels.Features.IAM;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Presistence;
using Blazored.LocalStorage;
using vetcms.ClientApplication.Common.IAM;
using vetcms.ClientApplication.Features.IAM.LoginUser;
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

            services.AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>();  // minden validatort adjon hozzá a shared lib-ből
            services.AddValidatorsFromAssemblyContaining<LoginUserClientCommand>(); // minden validatort adjon hozzá a client appból

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
