using System.Threading;
using System.Threading.Tasks;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.RegisterUser;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.UnitTests.Features.IAM
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddUser_WhenRequestIsValid()
        {
            // Arrange
            var request = new RegisterUserApiCommand
            {
                PhoneNumber = "123456789",
                Email = "test@example.com",
                Password = "password",
                Name = "Test User"
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task Handle_ShouldSetUserPropertiesCorrectly()
        {
            // Arrange
            var request = new RegisterUserApiCommand
            {
                PhoneNumber = "123456789",
                Email = "test@example.com",
                Password = "password",
                Name = "Test User"
            };

            User addedUser = null;
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => addedUser = user);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(addedUser);
            Assert.Equal(request.PhoneNumber, addedUser.PhoneNumber);
            Assert.Equal(request.Email, addedUser.Email);
            Assert.Equal(request.Name, addedUser.VisibleName);
            Assert.NotNull(addedUser.Password);
        }
    }
}
