using vetcms.ClientApplication;
using vetcms.SharedModels.Common.Behaviour;

namespace vetcms.BrowserPresentation
{
    /// <summary>
    /// A böngészőfüggőségek inicializálásáért felelős osztály.
    /// </summary>
    public static class BrowserDependencyInitializer
    {
        /// <summary>
        /// Hozzáadja a szükséges függőségeket a szolgáltatásokhoz.
        /// </summary>
        /// <param name="services">A szolgáltatások gyűjteménye.</param>
        /// <returns>A frissített szolgáltatások gyűjteménye.</returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddBrowserPresistence();
            services.AddClientApp();
            return services;
        }
    }
}
