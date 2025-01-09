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
    public class RegisterUserE2ETests : IClassFixture<WebApplicationFactory<WebApi.Program>>, IDisposable
    {
        private readonly WebApplicationFactory<WebApi.Program> _factory;
        private IServiceScope _scope;
        private ApplicationDbContext _dbContext;

        public RegisterUserE2ETests(WebApplicationFactory<WebApi.Program> factory)
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
                    _scope = serviceProvider.CreateScope();
                    _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Ensure the database is created
                    _dbContext.Database.EnsureCreated();
                });
            });
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnSuccess_WhenDataIsValid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserApiCommand
            {
                Email = "newuser@example.com",
                Password = "ValidPassword123",
                PhoneNumber = "12345678901",
                Name = "New User"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/register", command);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RegisterUserApiCommandResponse>();
            Assert.True(result.Success);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnFailure_WhenEmailIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserApiCommand
            {
                Email = "invalid-email",
                Password = "ValidPassword123",
                PhoneNumber = "12345678901",
                Name = "New User"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/register", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<RegisterUserApiCommandResponse>();
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnFailure_WhenEmailIsMissing()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserApiCommand
            {
                Password = "ValidPassword123",
                PhoneNumber = "12345678901",
                Name = "New User"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/register", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<RegisterUserApiCommandResponse>();
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnFailure_WhenPasswordIsMissing()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserApiCommand
            {
                Email = "newuser@example.com",
                PhoneNumber = "12345678901",
                Name = "New User"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/register", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<RegisterUserApiCommandResponse>();
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnFailure_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserApiCommand
            {
                Email = "newuser@example.com",
                Password = "ValidPassword123",
                PhoneNumber = "12345",
                Name = "New User"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/iam/register", command);

            // Assert
            var result = await response.Content.ReadFromJsonAsync<RegisterUserApiCommandResponse>();
            Assert.False(result.Success);
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