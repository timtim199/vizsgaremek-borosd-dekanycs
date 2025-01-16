using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Common.Abstract;
using vetcms.SharedModels.Features.IAM;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using vetcms.ServerApplication.Features.IAM;
using Microsoft.Extensions.Configuration;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.ServerApplication.Common.Abstractions.IAM;

namespace vetcms.ServerApplicationTests.E2ETests.Features.IAM
{
    public class AssignUserPermissionE2ETests : IClassFixture<WebApplicationFactory<WebApi.Program>>, IDisposable
    {
        private readonly WebApplicationFactory<WebApi.Program> _factory;
        private IServiceScope _scope;
        private ApplicationDbContext _dbContext;
        private IAuthenticationCommon _authenticationCommon;

        public AssignUserPermissionE2ETests(WebApplicationFactory<WebApi.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the real database with an in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_AssignPermission");
                    });

                    // Build the service provider
                    var serviceProvider = services.BuildServiceProvider();
                    _scope = serviceProvider.CreateScope();
                    _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    _authenticationCommon = _scope.ServiceProvider.GetRequiredService<IAuthenticationCommon>();

                    // Ensure the database is created
                    _dbContext.Database.EnsureCreated();
                });
            });

            // Ensure the web host is created before running any test cases
            _factory.CreateClient();
        }

        private string SeedDatabase()
        {
            var guid = Guid.NewGuid().ToString();
            var user = new User
            {
                Email = $"{guid}@example.com",
                Password = PasswordUtility.HashPassword("ValidPassword123"),
                PhoneNumber = "1234567890",
                VisibleName = guid
            };
            _dbContext.Set<User>().Add(user);
            _dbContext.SaveChanges();

            return guid;
        }

        private string SeedExecutorUser(bool withPermission = true)
        {
            var guid = Guid.NewGuid().ToString();
            var user = new User
            {
                Email = $"{guid}@executor.com",
                Password = PasswordUtility.HashPassword("ExecutorPassword123"),
                PhoneNumber = "0987654321",
                VisibleName = "Executor User",
            };
            if(withPermission)
            {
                user.OverwritePermissions(new EntityPermissions().AddFlag(PermissionFlags.CAN_ASSIGN_PERMISSIONS));
            }
            _dbContext.Set<User>().Add(user);
            _dbContext.SaveChanges();

            return guid;
        }

        private void ClearDatabase(string guid)
        {
            //_dbContext.Set<User>().RemoveRange(_dbContext.Set<User>().Where(u => u.Email.Contains(guid)));
            //_dbContext.SaveChanges();
        }

        private string GenerateBearerToken(string guid)
        {
            var user = _dbContext.Set<User>().First(u => u.Email.Contains(guid));
            return _authenticationCommon.GenerateAccessToken(user);
        }

        [Fact]
        public async Task AssignUserPermission_ShouldReturnSuccess_WhenRequestIsValid()
        {
            // Arrange
            var guid = SeedDatabase();
            var executorGuid = SeedExecutorUser();
            var client = _factory.CreateClient();
            var permission = new EntityPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN);

            var command = new AssignUserPermissionApiCommand
            {
                Id = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com").Id,
                PermissionSet = permission.ToString()
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(executorGuid));

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/assign-permission", command);

            // Assert
            //response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AssignUserPermissionApiCommandResponse>();
            Assert.True(result.Success);

            ClearDatabase(guid);
            ClearDatabase(executorGuid);
        }

        [Fact]
        public async Task AssignUserPermission_ShouldReturnFailure_WhenUserIdIsInvalid()
        {
            // Arrange
            var guid = SeedDatabase();
            var executorGuid = SeedExecutorUser();
            var client = _factory.CreateClient();
            var permission = new EntityPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN);

            var command = new AssignUserPermissionApiCommand
            {
                Id = 999,
                PermissionSet = permission.ToString()
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(executorGuid));

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/assign-permission", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<AssignUserPermissionApiCommandResponse>();
            Assert.False(result.Success);

            ClearDatabase(guid);
            ClearDatabase(executorGuid);
        }

        [Fact]
        public async Task AssignUserPermission_ShouldReturnFailure_WhenPermissionSetIsInvalid()
        {
            // Arrange
            var guid = SeedDatabase();
            var executorGuid = SeedExecutorUser();
            var client = _factory.CreateClient();
            var command = new AssignUserPermissionApiCommand
            {
                Id = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com").Id,
                PermissionSet = ""
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(executorGuid));

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/assign-permission", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<AssignUserPermissionApiCommandResponse>();
            Assert.False(result.Success);

            ClearDatabase(guid);
            ClearDatabase(executorGuid);
        }

        [Fact]
        public async Task AssignUserPermission_ShouldReturnFailure_WhenUserLacksPermission()
        {
            // Arrange
            var guid = SeedDatabase();
            var executorGuid = SeedExecutorUser(false);
            var client = _factory.CreateClient();
            var permission = new EntityPermissions().RemoveFlag(PermissionFlags.CAN_ASSIGN_PERMISSIONS);

            var command = new AssignUserPermissionApiCommand
            {
                Id = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com").Id,
                PermissionSet = permission.ToString()
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(executorGuid));

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/assign-permission", command);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            ClearDatabase(guid);
            ClearDatabase(executorGuid);
        }

        [Fact]
        public async Task AssignUserPermission_ShouldReturnFailure_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var guid = SeedDatabase();
            var client = _factory.CreateClient();
            var permission = new EntityPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN);

            var command = new AssignUserPermissionApiCommand
            {
                Id = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com").Id,
                PermissionSet = permission.ToString()
            };

            client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/assign-permission", command);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            ClearDatabase(guid);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Database.EnsureDeleted();
                _dbContext.Dispose();
                _scope.Dispose();
            }
        }
    }
}
