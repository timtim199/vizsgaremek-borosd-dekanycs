using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Common.Behaviour;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using Microsoft.AspNetCore.Routing;
using vetcms.ServerApplication.Common.IAM;
using vetcms.SharedModels.Features.IAM;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Common;
using vetcms.ServerApplication.Infrastructure.Communication.Mail;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Common.Behaviour;
using vetcms.ServerApplication.Features.IAM.SuperUser;

namespace vetcms.ServerApplication
{
    public static class ServerDependencyInitializer

    {
        public static bool IsTestEnviroment { get => IsRunningUnitTest();  }
        public static IServiceCollection AddServerApp(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ServerDependencyInitializer).Assembly);

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ServerDependencyInitializer).Assembly);
                options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                options.AddOpenBehavior(typeof(UserValidationBehavior<,>));
                options.AddOpenBehavior(typeof(PermissionRequirementBehaviour<,>));
            });

            services.Configure<RouteOptions>(o =>
            {
                o.LowercaseUrls = true;
            });

            services.AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>(); // minden validatort adjon hozzá a shared lib-ből
            services.AddValidatorsFromAssemblyContaining<ApplicationDbContext>();  // minden validatort adjon hozzá a server appból

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration baseConfiguration)
        {
            var configuration = SecuredConfiguration.FromPlainConfiguration(baseConfiguration);
            if(!IsTestEnviroment)
            {
                services.InitializeDatabaseDriver(configuration);
            }
            services.InitializeRepositoryComponents(configuration);
            services.AddCommunicationServices(configuration);
            services.AddScoped<IAuthenticationCommon, AuthenticationCommon>();
            services.AddHostedService<SuperUserInitializer>();
            return services;
        }

        private static void AddCommunicationServices(this IServiceCollection services, SecuredConfiguration configuration)
        {
            services.AddSingleton<IMailDeliveryProviderWrapper>(p =>
                new MailgunServiceWrapper(
                    configuration.GetValue<string>("MailServices:Mailgun:Domain"),
                    configuration.GetValue<string>("MailServices:Mailgun:ApiKey"),
                    configuration.GetValue<string>("MailServices:Mailgun:Sender")
                    )
            );
            services.AddSingleton<IMailService, MailService>();
        }

        private static void InitializeDatabaseDriver(this IServiceCollection services, SecuredConfiguration configuration)
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

        private static void InitializeRepositoryComponents(this IServiceCollection services, SecuredConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFirstTimeAuthenticationCodeRepository, FirstTimeAuthenticationCodeRepository>();
        }

        private static void AddInMemoryDatabase(this IServiceCollection services, SecuredConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("vetcms"));
        }

        private static void AddMsSqlServerDatabase(this IServiceCollection services, SecuredConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("mssql"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        private static void AddSqliteDataBase(this IServiceCollection services, SecuredConfiguration configuration)
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

        public static bool IsRunningUnitTest()
        {
            var testAssemblies = new[] { "xunit", "nunit", "mstest" };
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            return loadedAssemblies.Any(a => testAssemblies.Any(t => a.FullName.ToLower().Contains(t)));
        }
    }
}