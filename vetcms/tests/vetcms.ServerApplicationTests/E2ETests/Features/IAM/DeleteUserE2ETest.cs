using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using vetcms.ServerApplication;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.E2ETests.Features.IAM
{
    public class DeleteUserE2ETest : IClassFixture<WebApplicationFactory<WebApi.Program>>
    {
        private readonly WebApplicationFactory<WebApi.Program> _factory;
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticationCommon _authenticationCommon;

        public DeleteUserE2ETest(WebApplicationFactory<WebApi.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });

                    services.AddScoped<IAuthenticationCommon, AuthenticationCommon>();

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                        db.Database.EnsureCreated();
                    }
                });
            });

            _client = _factory.CreateClient();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            using (var scope = _factory.Services.CreateScope())
            {
                _authenticationCommon = scope.ServiceProvider.GetRequiredService<IAuthenticationCommon>();
            }
        }

        private string SeedAdminUser()
        {
            var adminUserId = 10000;
            var guid = Guid.NewGuid().ToString();
            var adminUser = new User
            {
                Id = adminUserId,
                Email = $"{guid}@admin.com",
                Password = PasswordUtility.HashPassword("AdminPassword123"),
                PhoneNumber = "06111111111",
                VisibleName = "Admin User",
            };

            adminUser.OverwritePermissions(new EntityPermissions().AddFlag(PermissionFlags.CAN_DELETE_USERS));
            _dbContext.Set<User>().Add(adminUser);
            _dbContext.SaveChangesAsync();
            return guid;
        }

        private string GenerateBearerToken(string guid)
        {
            var user = _dbContext.Set<User>().First(u => u.Email.Contains(guid));
            return _authenticationCommon.GenerateAccessToken(user);
        }

        [Fact]
        public async Task DeleteUserById_Success()
        {
            // Ensure the database is in a clean state before the test
            await _dbContext.Database.EnsureDeletedAsync();

            // Arrange
            var adminUserGuid = SeedAdminUser(); // Create an admin user
            int userId = 1; // ID of the user to be deleted

            // Add a user to the database
            _dbContext.Set<User>().Add(new User
            {
                Id = userId,
                Email = $"test{userId}@test.com",
                Password = PasswordUtility.HashPassword("test"),
                PhoneNumber = "06111111111",
                VisibleName = "test"
            });
            await _dbContext.SaveChangesAsync();

            var deleteUserCommand = new DeleteUserApiCommand(userId);

            // Add authorization
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(adminUserGuid));

            // Act
            var request = new HttpRequestMessage
            {
                Content = JsonContent.Create(deleteUserCommand), // Ensure correct serialization
                Method = HttpMethod.Delete,
                RequestUri = new Uri("/api/v1/iam/user", UriKind.Relative)
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful

            var result = await response.Content.ReadFromJsonAsync<DeleteUserApiCommandResponse>();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("", result.Message);

            // Verify the user is deleted
            var userDeleted = await _dbContext.Set<User>().FindAsync(userId);
            Assert.Null(userDeleted);
        }


        [Fact]
        public async Task DeleteUserById_NotFound()
        {
            // Arrange
            var adminUser = SeedAdminUser();
            var userId = 999; // Assuming this user does not exist
            var deleteUserCommand = new DeleteUserApiCommand(new List<int> { userId });
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(adminUser));

            // Act
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/iam/user")
            {
                Content = JsonContent.Create(deleteUserCommand), // Ensure correct serialization
            };

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<DeleteUserApiCommandResponse>();

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal($"Nem létező felhasználó ID(s): {userId}", result.Message);
        }

        [Fact]
        public async Task DeleteMultipleUsersByIds_Success()
        {
            // Arrange
            var adminGuid = SeedAdminUser();
            var userIds = new List<int> { 1, 2, 3 };
            var client = _factory.CreateClient();
            foreach (var userId in userIds)
            {
                _dbContext.Set<User>().Add(new User
                {
                    Id = userId,
                    Email = $"test{userId}@test.com",
                    Password = PasswordUtility.HashPassword("test"),
                    PhoneNumber = "06111111111",
                    VisibleName = "test"
                });
            }
            await _dbContext.SaveChangesAsync();

            var deleteUserCommand = new DeleteUserApiCommand(userIds);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(adminGuid));

            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/iam/user")
            {
                Content = JsonContent.Create(deleteUserCommand), // Ensure correct serialization
            };

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadFromJsonAsync<DeleteUserApiCommandResponse>();

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("", result.Message);
            foreach (var userId in userIds)
            {
                var user = await _dbContext.Set<User>().FindAsync(userId);
                Assert.Null(user);
            }
        }

        [Fact]
        public async Task DeleteMultipleUsersByIds_NotFound()
        {
            // Arrange
            var adminGuid = SeedAdminUser();
            var userIds = new List<int> { 999, 1000, 1001 }; // Assuming these users do not exist
            var deleteUserCommand = new DeleteUserApiCommand(userIds);
            var client = _factory.CreateClient();

            // Add authorization header
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateBearerToken(adminGuid));

            // Act
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/iam/user")
            {
                Content = JsonContent.Create(deleteUserCommand), // Ensure correct serialization
            };

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<DeleteUserApiCommandResponse>();

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal($"Nem létező felhasználó ID(s): {string.Join(",", userIds)}", result.Message);
        }



    }
}
