using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.IAM.DeleteUser;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.IntegrationTests.Features.IAM
{
    public class DeleteUserCommandHandlerIntegrationTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRequestHandler<DeleteUserApiCommand, DeleteUserApiCommandResponse> _handler;

        public DeleteUserCommandHandlerIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            _handler = new DeleteUserCommandHandler(_userRepository);
        }

        [Fact]
        public async Task DeleteSingleUser_Success()
        {
            // Arrange
            var userId = 1;
            _dbContext.Set<User>().Add(new User
            {
                Id = userId,
                Email = "test01@test.com",
                Password = "test",
                PhoneNumber = "06111111111",
                VisibleName = "test"
            });
            await _dbContext.SaveChangesAsync();

            var command = new DeleteUserApiCommand(new List<int> { userId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("", result.Message);
            Assert.Null(await _dbContext.Set<User>().FindAsync(userId));
        }

        [Fact]
        public async Task DeleteMultipleUsers_Success()
        {
            // Arrange
            var userIds = new List<int> { 1, 2, 3 };
            foreach (var userId in userIds)
            {
                _dbContext.Set<User>().Add(new User 
                { Id = userId, 
                    Email = "test01@test.com", 
                    Password = "test", 
                    PhoneNumber = "06111111111", 
                    VisibleName = "test" 
                });
            }
            await _dbContext.SaveChangesAsync();

            var command = new DeleteUserApiCommand(userIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("", result.Message);
            foreach (var userId in userIds)
            {
                Assert.Null(await _dbContext.Set<User>().FindAsync(userId));
            }
        }

        [Fact]
        public async Task DeleteSingleUser_NotFound()
        {
            // Arrange
            var userId = 999; // Assuming this user does not exist
            var command = new DeleteUserApiCommand(new List<int> { userId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Nem létező felhasználó ID(s): {userId}", result.Message);
        }

        [Fact]
        public async Task DeleteMultipleUsers_NotFound()
        {
            // Arrange
            var userIds = new List<int> { 999, 1000, 1001 }; // Assuming these users do not exist
            var command = new DeleteUserApiCommand(userIds);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Nem létező felhasználó ID(s): {string.Join(",", userIds)}", result.Message);
        }
    }
}