using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.E2ETests.Features.IAM
{
    public class ResetPasswordE2ETests : IClassFixture<WebApplicationFactory<WebApi.Program>>, IDisposable
    {
        private readonly WebApplicationFactory<WebApi.Program> _factory;
        private IServiceScope _scope;
        private ApplicationDbContext _dbContext;
        private Mock<IMailService> _mockMailService;

        public ResetPasswordE2ETests(WebApplicationFactory<WebApi.Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_ResetPasswordE2E");
                    });

                    _mockMailService = new Mock<IMailService>();
                    services.AddSingleton(_mockMailService.Object);

                    var serviceProvider = services.BuildServiceProvider();
                    _scope = serviceProvider.CreateScope();
                    _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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

        private void ClearDatabase(string guid)
        {
            _dbContext.Set<User>().RemoveRange(_dbContext.Set<User>().Where(u => u.Email == $"{guid}@example.com"));
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task BeginResetPassword_ShouldReturnSuccess_WhenUserExists()
        {
            var client = _factory.CreateClient();
            var guid = SeedDatabase();
            var command = new BeginResetPasswordApiCommand { Email = $"{guid}@example.com" };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/begin", command);

            var result = await response.Content.ReadFromJsonAsync<BeginResetPasswordApiCommandResponse>();
            Assert.True(result.Success);
            _mockMailService.Verify(m => m.SendPasswordResetEmailAsync(It.IsAny<PasswordReset>()), Times.Once);

            ClearDatabase(guid);
        }

        [Fact]
        public async Task BeginResetPassword_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            var client = _factory.CreateClient();
            var command = new BeginResetPasswordApiCommand { Email = "nonexistent@example.com" };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/begin", command);

            var result = await response.Content.ReadFromJsonAsync<BeginResetPasswordApiCommandResponse>();
            Assert.False(result.Success);
            Assert.Equal("Nincs ilyen felhasználó.", result.Message);
            _mockMailService.Verify(m => m.SendPasswordResetEmailAsync(It.IsAny<PasswordReset>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldReturnSuccess_WhenCodeIsValid()
        {
            var client = _factory.CreateClient();
            var guid = SeedDatabase();
            var user = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com");
            user.PasswordResets.Add(new PasswordReset { Email = $"{guid}@example.com", Code = "ABC123", Deleted = false });
            _dbContext.SaveChanges();

            var command = new ConfirmResetPasswordApiCommand
            {
                Email = $"{guid}@example.com",
                ConfirmationCode = "ABC123",
                NewPassword = "NewValidPassword123"
            };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/confirm", command);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ConfirmResetPasswordApiCommandResponse>();
            Assert.True(result.Success);

            ClearDatabase(guid);
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldReturnFailure_WhenCodeIsInvalid()
        {
            var client = _factory.CreateClient();
            var guid = SeedDatabase();
            var user = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com");
            user.PasswordResets.Add(new PasswordReset { Email = $"{guid}@example.com", Code = "ABC123", Deleted = false });
            _dbContext.SaveChanges();

            var command = new ConfirmResetPasswordApiCommand
            {
                Email = $"{guid}@example.com",
                ConfirmationCode = "CBA321",
                NewPassword = "NewValidPassword123"
            };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/confirm", command);
            var result = await response.Content.ReadFromJsonAsync<ConfirmResetPasswordApiCommandResponse>();
            Assert.False(result.Success);
            Assert.Equal("Hibás kód!", result.Message);

            ClearDatabase(guid);
        }

        [Fact]
        public async Task BeginResetPassword_ShouldFailValidation_WhenEmailIsInvalid()
        {
            var client = _factory.CreateClient();
            var command = new BeginResetPasswordApiCommand { Email = "invalid-email" };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/begin", command);

            var result = await response.Content.ReadFromJsonAsync<ConfirmResetPasswordApiCommandResponse>();
            Assert.Contains("A mezőket helyesen kell kitölteni", result.Message);
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldFailValidation_WhenEmailIsInvalid()
        {
            var client = _factory.CreateClient();
            var command = new ConfirmResetPasswordApiCommand
            {
                Email = "invalid-email",
                ConfirmationCode = "ABC123",
                NewPassword = "NewValidPassword123"
            };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/confirm", command);

            var result = await response.Content.ReadFromJsonAsync<ConfirmResetPasswordApiCommandResponse>();
            Assert.Contains("A mezőket helyesen kell kitölteni", result.Message);
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldFailValidation_WhenConfirmationCodeIsInvalid()
        {
            var client = _factory.CreateClient();
            var command = new ConfirmResetPasswordApiCommand
            {
                Email = "test@example.com",
                ConfirmationCode = "123",
                NewPassword = "NewValidPassword123"
            };

            var response = await client.PostAsJsonAsync("/api/v1/iam/reset-password/confirm", command);

            var result = await response.Content.ReadFromJsonAsync<ConfirmResetPasswordApiCommandResponse>();
            Assert.Contains("A mezőket helyesen kell kitölteni", result.Message);
        }

        public void Dispose()
        {
            _scope.Dispose();
            _dbContext.Dispose();
        }
    }
}
