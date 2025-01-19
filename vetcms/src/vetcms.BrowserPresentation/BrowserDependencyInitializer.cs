using vetcms.ClientApplication;
using vetcms.SharedModels.Common.Behaviour;

namespace vetcms.BrowserPresentation
{
    public static class BrowserDependencyInitializer
    {
        /// <summary>
        /// Adds the necessary dependencies to the services.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <returns>The updated collection of services.</returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddBrowserPresistence();
            services.AddClientApp();
            return services;
        }
    }
}
