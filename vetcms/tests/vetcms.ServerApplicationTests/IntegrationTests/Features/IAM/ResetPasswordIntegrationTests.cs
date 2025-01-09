using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.ResetPassword;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.IntegrationTests.Features.IAM
{
    public class ResetPasswordIntegrationTests : IAsyncLifetime
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IRequestHandler<ConfirmResetPasswordApiCommand, ConfirmResetPasswordApiCommandResponse> _confirmHandler;
        private readonly IRequestHandler<BeginResetPasswordApiCommand, BeginResetPasswordApiCommandResponse> _beginHandler;

        public ResetPasswordIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_ResetPasswordIntegrationTests")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            _mailService = new Mock<IMailService>().Object;
            _confirmHandler = new ConfirmResetPasswordCommandHandler(_userRepository);
            _beginHandler = new BeginResetPasswordCommandHandler(_userRepository, _mailService);
        }

        public async Task InitializeAsync()
        {
            // Initialize database if needed
        }

        public async Task DisposeAsync()
        {
            // Clean up database after each test
            await _dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldReturnSuccess_WhenCodeIsValid()
        {
            // Arrange
            var user = new User 
            { 
                Email = "test@example.com",
                Password = "initialpassword",
                PhoneNumber = "1234567890",
                VisibleName = "Test User",
                PasswordResets = new List<PasswordReset>()
            };
            user.PasswordResets.Add(new PasswordReset { Email = "test@example.com", Code = "ABC123", Deleted = false });
            await _userRepository.AddAsync(user);

            var request = new ConfirmResetPasswordApiCommand
            {
                Email = "test@example.com",
                ConfirmationCode = "ABC123",
                NewPassword = "newpassword"
            };

            // Act
            var result = await _confirmHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            //Cleanup
            await _userRepository.DeleteAsync(user.Id);
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldReturnFailure_WhenCodeIsInvalid()
        {
            // Arrange
            var user = new User 
            { 
                Email = "test@example.com",
                Password = "initialpassword",
                PhoneNumber = "1234567890",
                VisibleName = "Test User",
                PasswordResets = new List<PasswordReset>()
            };
            user.PasswordResets.Add(new PasswordReset { Email = "test@example.com", Code = "ABC123", Deleted = false });
            await _userRepository.AddAsync(user);

            var request = new ConfirmResetPasswordApiCommand
            {
                Email = "test@example.com",
                ConfirmationCode = "INVALID",
                NewPassword = "newpassword"
            };

            // Act
            var result = await _confirmHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Hibás kód!", result.Message);

            //Cleanup
            await _userRepository.DeleteAsync(user.Id);
        }

        [Fact]
        public async Task BeginResetPassword_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            var user = new User 
            { 
                Email = "test@example.com",
                Password = "initialpassword",
                PhoneNumber = "1234567890",
                VisibleName = "Test User",
                PasswordResets = new List<PasswordReset>()
            };
            await _userRepository.AddAsync(user);

            var request = new BeginResetPasswordApiCommand
            {
                Email = "test@example.com"
            };

            // Act
            var result = await _beginHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);

            await _userRepository.DeleteAsync(user.Id);

        }

        [Fact]
        public async Task BeginResetPassword_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new BeginResetPasswordApiCommand
            {
                Email = "nonexistent@example.com"
            };

            // Act
            var result = await _beginHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nincs ilyen felhasználó.", result.Message);
        }

        [Fact]
        public void ConfirmResetPassword_ShouldFailValidation_WhenEmailIsInvalid()
        {
            // Arrange
            var validator = new ConfirmResetPasswordApiCommandValidator();
            var request = new ConfirmResetPasswordApiCommand
            {
                Email = "invalid-email",
                ConfirmationCode = "ABC123",
                NewPassword = "newpassword"
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public void ConfirmResetPassword_ShouldFailValidation_WhenConfirmationCodeIsInvalid()
        {
            // Arrange
            var validator = new ConfirmResetPasswordApiCommandValidator();
            var request = new ConfirmResetPasswordApiCommand
            {
                Email = "test@example.com",
                ConfirmationCode = "123",
                NewPassword = "newpassword"
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "ConfirmationCode");
        }

        [Fact]
        public void BeginResetPassword_ShouldFailValidation_WhenEmailIsInvalid()
        {
            // Arrange
            var validator = new BeginResetPasswordApiCommandValidator();
            var request = new BeginResetPasswordApiCommand
            {
                Email = "invalid-email"
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }
    }
}
