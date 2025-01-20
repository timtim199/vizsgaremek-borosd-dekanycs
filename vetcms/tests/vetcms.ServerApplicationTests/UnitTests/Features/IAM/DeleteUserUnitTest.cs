// File: tests/vetcms.ServerApplicationTests/UnitTests/Features/IAM/DeleteUserCommandHandlerUnitTests.cs

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Exceptions;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.DeleteUser;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.UnitTests.Features.IAM
{
    public class DeleteUserCommandHandlerUnitTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_SingleUserExists_DeletesUser()
        {
            // Arrange
            var userId = 1;
            var command = new DeleteUserApiCommand(new List<int> { userId });

            _userRepositoryMock.Setup(repo => repo.ExistAsync(userId)).ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId)).ReturnsAsync(new User { Id = userId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("", result.Message);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Handle_MultipleUsersExist_DeletesUsers()
        {
            // Arrange
            var userIds = new List<int> { 1, 2, 3 };
            var command = new DeleteUserApiCommand(userIds);

            foreach (var userId in userIds)
            {
                _userRepositoryMock.Setup(repo => repo.ExistAsync(userId)).ReturnsAsync(true);
                _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId)).ReturnsAsync(new User { Id = userId });
            }

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("", result.Message);
            foreach (var userId in userIds)
            {
                _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
            }
        }

        [Fact]
        public async Task Handle_SingleUserNotFound_ReturnsErrorMessage()
        {
            // Arrange
            var userId = 1;
            var command = new DeleteUserApiCommand(new List<int> { userId });

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, false)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Nem létező felhasználó ID(s): {userId}", result.Message);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Never);
        }

        [Fact]
        public async Task Handle_MultipleUsersNotFound_ReturnsErrorMessage()
        {
            // Arrange
            var userIds = new List<int> { 1, 2, 3 };
            var command = new DeleteUserApiCommand(userIds);

            foreach (var userId in userIds)
            {
                _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId, false)).ReturnsAsync((User)null);
            }

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Nem létező felhasználó ID(s): {string.Join(",", userIds)}", result.Message);
            foreach (var userId in userIds)
            {
                _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Never);
            }
        }
    }
}
