using vetcms.ClientApplication;
using vetcms.SharedModels.Common.Behaviour;

namespace vetcms.BrowserPresentation
{
    public static class BrowserDependencyInitializer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddClientApp();
            return services;
        }

        private static IServiceCollection InitMediatR(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.AddOpenBehavior(typeof(ValidationBehaviour<,>));


                options.RegisterServicesFromAssembly(typeof(BrowserDependencyInitializer).Assembly);
                options.RegisterServicesFromAssembly(typeof(ClientDependencyInitializer).Assembly);
            });
            return services;
        }
    }
}
