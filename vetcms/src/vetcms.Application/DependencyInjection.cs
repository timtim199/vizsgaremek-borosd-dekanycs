using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Infrastructure.Presistence;
using FluentValidation;
using vetcms.SharedModels.Common.Behaviour;

namespace vetcms.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });


            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            InitializeDatabaseDriver(services, configuration);
            return services;
        }

        private static void InitializeDatabaseDriver(this IServiceCollection services, IConfiguration configuration)
        {
            switch ((configuration.GetValue<string>("database_driver")))
            {
                case "InMemory":
                    AddInMemoryDatabase(services, configuration);
                    break;
                case "MsSqlServer":
                    AddMsSqlServerDatabase(services, configuration);
                    break;
                case "sqlite":
                    Console.WriteLine("sqlite");
                    AddSqliteDataBase(services, configuration);
                    break;
                default:
                    break;
            }
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