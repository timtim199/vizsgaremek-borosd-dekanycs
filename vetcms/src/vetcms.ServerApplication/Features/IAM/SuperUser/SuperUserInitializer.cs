using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.ServerApplication.Features.IAM.SuperUser
{
    public class SuperUserInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public SuperUserInitializer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            bool isSuperUserEnabled = _configuration.GetValue<bool>("SuperUser:Enabled");
            string superUserEmail = _configuration.GetValue<string>("SuperUser:Email");
            string superUserPassword = _configuration.GetValue<string>("SuperUser:Password");

            if (!isSuperUserEnabled)
            {
                Console.WriteLine("SuperUser creation is disabled in the configuration.");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var users = context.Set<User>();
                User superUserEntity;

                if (!users.Any(u => u.Email == superUserEmail))
                {
                    superUserEntity = new User
                    {
                        Email = superUserEmail,
                        Password = PasswordUtility.HashPassword(superUserPassword),
                        FirstName = "Super",
                        LastName = "User",
                        VisibleName = "Super User",
                        Created = DateTime.Now,
                        LastModified = DateTime.Now,
                        CreatedByUserId = 0,
                        LastModifiedByUserId = 0,
                    };

                    users.Add(superUserEntity);
                    await context.SaveChangesAsync();
                }
                else
                {
                    superUserEntity = users.First(u => u.Email == superUserEmail);
                }

                EntityPermissions permissions = new();
                permissions.AddFlag(Enum.GetValues<PermissionFlags>());
                superUserEntity.OverwritePermissions(permissions);
                superUserEntity.Password = PasswordUtility.HashPassword(superUserPassword, superUserEmail);
                users.Update(superUserEntity);
                await context.SaveChangesAsync();

                Console.WriteLine("##################################################");
                Console.WriteLine("##################################################");
                Console.WriteLine("############# [SUPERUSER GENERATED] ##############");
                Console.WriteLine($"[Email: {superUserEmail} ]");
                Console.WriteLine($"[Password: {superUserPassword} ]");
                Console.WriteLine("############# [×××] ##############################");
                Console.WriteLine("##################################################");
                Console.WriteLine("##################################################");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
