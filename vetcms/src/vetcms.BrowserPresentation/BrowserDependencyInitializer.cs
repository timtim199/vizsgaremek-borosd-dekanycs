using vetcms.ClientApplication;
using vetcms.SharedModels.Common.Behaviour;

namespace vetcms.BrowserPresentation
{
    public static class BrowserDependencyInitializer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddBrowserPresistence();
            services.AddClientApp();
            return services;
        }
    }
}
