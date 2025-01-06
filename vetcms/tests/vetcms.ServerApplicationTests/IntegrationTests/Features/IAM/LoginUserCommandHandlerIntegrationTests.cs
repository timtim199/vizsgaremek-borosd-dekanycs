using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM;
using vetcms.ServerApplication.Features.IAM.LoginUser;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.SharedModels.Features.IAM;
using Xunit;
//[assembly: InternalsVisibleTo("vetcms.ServerApplicationTests")]
namespace vetcms.ServerApplicationTests.IntegrationTests.Features.IAM
{
    public class LoginUserCommandHandlerIntegrationTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly Mock<IAuthenticationCommon> _authenticationCommonMock;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRequestHandler<LoginUserApiCommand, LoginUserApiCommandResponse> _handler;

        public LoginUserCommandHandlerIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            _authenticationCommonMock = new Mock<IAuthenticationCommon>();
            _handler = new LoginUserCommandHandler(_userRepository, _authenticationCommonMock.Object);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsFailureResponse()
        {
            // Arrange
            var command = new LoginUserApiCommand() { Email = "none@none.com", Password = "hashed" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nem létező felhasználó", result.Message);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ReturnsFailureResponse()
        {
            // Arrange
            string password = PasswordUtility.HashPassword("hashedpassword");
            var user = new User
            {
                Email = "user@example.com",
                Password = password,
                PhoneNumber = "1234567890",
                VisibleName = "Test User"
            };
            _dbContext.Set<User>().Add(user);
            await _dbContext.SaveChangesAsync();

            var command = new LoginUserApiCommand() { Email = "user@example.com", Password = "invalidpassword" };

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
            string password = PasswordUtility.HashPassword("hashedpassword");
            var user = new User
            {
                Email = "user@example.com",
                Password = password,
                PhoneNumber = "1234567890",
                VisibleName = "Test User"
            };
            _dbContext.Set<User>().Add(user);
            await _dbContext.SaveChangesAsync();

            var command = new LoginUserApiCommand() { Email = "user@example.com", Password = "hashedpassword" };
            _authenticationCommonMock.Setup(auth => auth.GenerateAccessToken(It.IsAny<User>(), default)).Returns("token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("token", result.AccessToken);
        }

        [Fact]
        public async Task Handle_UserWithoutLoginPermission_ReturnsFailureResponse()
        {
            // Arrange
            string password = PasswordUtility.HashPassword("password");
            var user = new User
            {
                Email = "user@example.com",
                Password = password,
                PhoneNumber = "1234567890",
                VisibleName = "Test User"
            };
            user.GetPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN);
            _dbContext.Set<User>().Add(user);
            await _dbContext.SaveChangesAsync();

            var command = new LoginUserApiCommand() { Email = "user@example.com", Password = "password" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Hibás bejelentkezési adatok.", result.Message);
        }
    }
}
