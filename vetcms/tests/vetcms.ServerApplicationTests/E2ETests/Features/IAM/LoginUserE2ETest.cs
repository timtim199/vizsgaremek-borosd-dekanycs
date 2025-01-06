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

namespace vetcms.ServerApplicationTests.E2ETests.Features.IAM
{
    public class LoginUserE2ETests : IClassFixture<WebApplicationFactory<WebApi.Program>>
    {
        private readonly WebApplicationFactory<WebApi.Program> _factory;

        public LoginUserE2ETests(WebApplicationFactory<WebApi.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {

                builder.ConfigureServices(services =>
                {
                    // Replace the real database with an in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    // Build the service provider
                    var serviceProvider = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        // Ensure the database is created
                        dbContext.Database.EnsureCreated();

                        // Seed the database with test data
                        SeedDatabase(dbContext);
                    }
                });
            });
        }

        private void SeedDatabase(ApplicationDbContext dbContext)
        {
            var password = PasswordUtility.HashPassword("ValidPassword123");
            // Add a test user to the database
            var testUser = new User
            {
                Email = "test@example.com",
                Password = password, // Ensure this is hashed if your application uses hashed passwords
                PhoneNumber = "1234567890", // Provide a valid phone number
                VisibleName = "Test User" // Provide a visible name
            };
            dbContext.Set<User>().Add(testUser);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task LoginUser_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new LoginUserApiCommand
            {
                Email = "test@example.com",
                Password = "ValidPassword123"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/login", command);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<LoginUserApiCommandResponse>();
            Assert.True(result.Success);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnFailure_WhenCredentialsAreInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new LoginUserApiCommand
            {
                Email = "test@example.com",
                Password = "InvalidPassword"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/login", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<LoginUserApiCommandResponse>();
            Assert.False(result.Success);
        }
        [Fact]
        public async Task LoginUser_ShouldReturnFailure_WhenEmailIsMissing()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new LoginUserApiCommand
            {
                Password = "ValidPassword123"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/login", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<LoginUserApiCommandResponse>();
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnFailure_WhenPasswordIsMissing()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new LoginUserApiCommand
            {
                Email = "test@example.com"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/login", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<LoginUserApiCommandResponse>();
            Assert.False(result.Success);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnFailure_WhenEmailIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new LoginUserApiCommand
            {
                Email = "invalid-email",
                Password = "ValidPassword123"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/login", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<LoginUserApiCommandResponse>();
            Assert.False(result.Success);
        }
    }
}
