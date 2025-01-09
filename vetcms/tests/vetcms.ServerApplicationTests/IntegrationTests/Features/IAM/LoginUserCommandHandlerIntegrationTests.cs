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
            SeedDatabase();
        }

        private string SeedDatabase()
        {
            var guid = Guid.NewGuid().ToString();
            var user = new User
            {
                Email = $"{guid}@example.com",
                Password = PasswordUtility.HashPassword("hashedpassword"),
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
        public async Task Handle_UserDoesNotExist_ReturnsFailureResponse()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var command = new LoginUserApiCommand() { Email = $"{guid}@none.com", Password = "hashed" };

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
            var guid = SeedDatabase();
            var command = new LoginUserApiCommand() { Email = $"{guid}@example.com", Password = "invalidpassword" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Hibás bejelentkezési adatok.", result.Message);

            ClearDatabase(guid);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            var guid = SeedDatabase();
            var command = new LoginUserApiCommand() { Email = $"{guid}@example.com", Password = "hashedpassword" };
            _authenticationCommonMock.Setup(auth => auth.GenerateAccessToken(It.IsAny<User>(), default)).Returns("token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("token", result.AccessToken);

            ClearDatabase(guid);
        }

        [Fact]
        public async Task Handle_UserWithoutLoginPermission_ReturnsFailureResponse()
        {
            // Arrange
            var guid = SeedDatabase();
            var user = _dbContext.Set<User>().First(u => u.Email == $"{guid}@example.com");
            user.OverwritePermissions(user.GetPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN));
            _dbContext.SaveChanges();

            var command = new LoginUserApiCommand() { Email = $"{guid}@example.com", Password = "hashedpassword" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Hibás bejelentkezési adatok.", result.Message);

            ClearDatabase(guid);
        }
    }
}
