using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Common.Behaviour;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using Microsoft.AspNetCore.Routing;
using vetcms.ServerApplication.Common.IAM;

namespace vetcms.ServerApplication
{
    public static class ServerDependencyInitializer

    {
        public static IServiceCollection AddServerApp(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ServerDependencyInitializer).Assembly);

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ServerDependencyInitializer).Assembly);

                options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.Configure<RouteOptions>(o =>
            {
                o.LowercaseUrls = true;
            });

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.InitializeDatabaseDriver(configuration);
            services.InitializeRepositoryComponents(configuration);
            services.AddScoped<AuthenticationCommon>();
            return services;
        }

        private static void InitializeDatabaseDriver(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration.GetValue<string>("database_driver"))
            {
                case "InMemory":
                    services.AddInMemoryDatabase(configuration);
                    break;
                case "MsSqlServer":
                    services.AddMsSqlServerDatabase(configuration);
                    break;
                case "sqlite":
                    Console.WriteLine("sqlite");
                    services.AddSqliteDataBase(configuration);
                    break;
                default:
                    Console.WriteLine("no db specified");
                    break;
            }
        }

        private static void InitializeRepositoryComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<UserRepository>();
        }

        private static void AddInMemoryDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("vetcms"));
        }

        private static void AddMsSqlServerDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("mssql"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        private static void AddSqliteDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    "Data Source=vetcms.db",
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)

                )
            );
        }
    }
}