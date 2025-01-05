using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.LoginUser;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;
using vetcms.SharedModels.Common.IAM.Authorization;
using Xunit;
using System.Runtime.CompilerServices;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Features.IAM;

namespace tests.vetcms.ServerApplicationTests.IntegrationTests.Features.IAM
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationCommon> _authenticationCommonMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationCommonMock = new Mock<IAuthenticationCommon>();
            _handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _authenticationCommonMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsFailureResponse()
        {
            // Arrange
            var command = new LoginUserApiCommand { Email = "nonexistent@example.com", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Throws<InvalidOperationException>();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nem létező felhasználó", result.Message);
        }

        [Fact]
        public async Task Handle_UserHasNoLoginPermission_ReturnsFailureResponse()
        {
            // Arrange
            var user = new User { Email = "user@example.com", Password = "hashedpassword" };
            user.GetPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN);
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(user);
            var command = new LoginUserApiCommand { Email = "user@example.com", Password = "password" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Hibás bejelentkezési adatok.", result.Message);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailureResponse()
        {
            // Arrange
            var user = new User { Email = "user@example.com", Password = "hashedpassword" };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(user);
            var command = new LoginUserApiCommand { Email = "user@example.com", Password = "wrongpassword" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Hibás bejelentkezési adatok.", result.Message);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            string password = PasswordUtility.HashPassword("password");
            var user = new User { Email = "user@example.com", Password = password };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(It.IsAny<string>())).Returns(user);
            _authenticationCommonMock.Setup(auth => auth.GenerateAccessToken(It.IsAny<User>())).Returns("token");
            var command = new LoginUserApiCommand { Email = "user@example.com", Password = "password" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("token", result.AccessToken);
        }
    }
}