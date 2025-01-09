using Moq;
using System.Threading;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.ResetPassword;
using vetcms.ServerApplication.Infrastructure.Communication.Mail;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.UnitTests.Features.IAM
{
    public class ResetPasswordUnitTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly ConfirmResetPasswordCommandHandler _confirmHandler;
        private readonly BeginResetPasswordCommandHandler _beginHandler;

        public ResetPasswordUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mailServiceMock = new Mock<IMailService>();
            _confirmHandler = new ConfirmResetPasswordCommandHandler(_userRepositoryMock.Object);
            _beginHandler = new BeginResetPasswordCommandHandler(_userRepositoryMock.Object, _mailServiceMock.Object);
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldReturnSuccess_WhenCodeIsValid()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            user.PasswordResets.Add(new PasswordReset { Code = "ABC123", Deleted = false });
            _userRepositoryMock.Setup(repo => repo.HasUserByEmail(It.IsAny<string>())).Returns(true);
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(user);

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
        }

        [Fact]
        public async Task ConfirmResetPassword_ShouldReturnFailure_WhenCodeIsInvalid()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            user.PasswordResets.Add(new PasswordReset { Code = "ABC123", Deleted = false });
            _userRepositoryMock.Setup(repo => repo.HasUserByEmail(It.IsAny<string>())).Returns(true);
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(user);

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
        }

        [Fact]
        public async Task BeginResetPassword_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.HasUserByEmail(It.IsAny<string>())).Returns(true);
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(new User { Email = "test@example.com" });

            var request = new BeginResetPasswordApiCommand
            {
                Email = "test@example.com"
            };

            // Act
            var result = await _beginHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task BeginResetPassword_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.HasUserByEmail(It.IsAny<string>())).Returns(false);

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
        public async Task SendPasswordResetEmailAsync_ShouldCallSendEmailAsync()
        {
            // Arrange
            var mockMailServiceWrapper = new Mock<IMailDeliveryProviderWrapper>();
            var mailService = new MailService(mockMailServiceWrapper.Object);
            const string code = "123456";
            var passwordReset = new PasswordReset
            {
                Code = "123456",
                User = new User { Email = "test@example.com", VisibleName = "Test User" }
            };

            // Act
            await mailService.SendPasswordResetEmailAsync(passwordReset);

            // Assert
            mockMailServiceWrapper.Verify(m => m.SendEmailAsync(
                "test@example.com",
                "VETCMS: Elfelejtett jelszó",
                It.Is<string>(m => m.Contains(code))), Times.Once);
        }
    }
}
