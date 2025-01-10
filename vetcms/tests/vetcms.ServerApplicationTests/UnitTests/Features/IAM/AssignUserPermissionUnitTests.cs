using System.Threading;
using System.Threading.Tasks;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Exceptions;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.RegisterUser;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.UnitTests.Features.IAM
{
    public class AssignUserPermissionUnitTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AssignUserPermissionCommandHandler _handler;

        public AssignUserPermissionUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new AssignUserPermissionCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_UserExists_OverwritesPermissions()
        {
            // Arrange
            var userId = 1;
            var permissions = new EntityPermissions();
            permissions.AddFlag(PermissionFlags.CAN_LOGIN);
            var user = new User { Id = userId };
            var command = new AssignUserPermissionApiCommand { Id = userId, PermissionSet = permissions.ToString() };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, false)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Jogosultság sikeresen delegálva.", result.Message);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsErrorMessage()
        {
            // Arrange
            var userId = 1;
            var command = new AssignUserPermissionApiCommand { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, false)).ThrowsAsync(new NotFoundException());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nincs ilyen felhasználó.", result.Message);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
