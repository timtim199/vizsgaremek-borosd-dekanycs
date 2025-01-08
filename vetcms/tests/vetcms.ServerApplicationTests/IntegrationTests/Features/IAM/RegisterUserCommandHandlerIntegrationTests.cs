using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.RegisterUser;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.IntegrationTests.Features.IAM
{
    public class RegisterUserCommandHandlerIntegrationTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRequestHandler<RegisterUserApiCommand, RegisterUserApiCommandResponse> _handler;

        public RegisterUserCommandHandlerIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            _handler = new RegisterUserCommandHandler(_userRepository);
        }

        [Fact]
        public async Task Handle_UserAlreadyExists_ReturnsFailureResponse()
        {
            // Arrange
            var existingUser = new User
            {
                Email = "existing@user.com",
                Password = "hashedpassword",
                PhoneNumber = "1234567890",
                VisibleName = "Existing User"
            };
            _dbContext.Set<User>().Add(existingUser);
            await _dbContext.SaveChangesAsync();

            var command = new RegisterUserApiCommand() { Email = "existing@user.com", Password = "password", PhoneNumber = "1234567890", Name = "New User" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Az E-mail cím már foglalt.", result.Message);

            // Cleanup
            _dbContext.Set<User>().Remove(existingUser);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task Handle_NewUser_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new RegisterUserApiCommand() { Email = "new@user.com", Password = "password", PhoneNumber = "1234567890", Name = "New User" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            var user = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == "new@user.com");
            Assert.NotNull(user);
            Assert.Equal("New User", user.VisibleName);

            // Cleanup
            _dbContext.Set<User>().Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}