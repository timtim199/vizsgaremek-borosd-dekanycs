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
using vetcms.ServerApplication.Features.IAM.RegisterUser;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.IAM.Authorization;
using vetcms.SharedModels.Features.IAM;
using Xunit;

namespace vetcms.ServerApplicationTests.IntegrationTests.Features.IAM
{
    public class AssignUserPermissionIntegrationTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRequestHandler<AssignUserPermissionApiCommand, AssignUserPermissionApiCommandResponse> _handler;

        public AssignUserPermissionIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_AssignUserPermissionIntegrationTests")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            _handler = new AssignUserPermissionCommandHandler(_userRepository);
            SeedDatabase();
        }

        private User SeedDatabase()
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

            return user;
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
            var permission = new EntityPermissions().AddFlag(PermissionFlags.CAN_LOGIN);
            var command = new AssignUserPermissionApiCommand() { Id = 0, PermissionSet = permission.ToString() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nincs ilyen felhasználó.", result.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var user = SeedDatabase();
            var permission = new EntityPermissions().RemoveFlag(PermissionFlags.CAN_LOGIN);
            var command = new AssignUserPermissionApiCommand() { Id = user.Id, PermissionSet = permission.ToString() };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Jogosultság sikeresen delegálva.", result.Message);
            Assert.Matches(permission.ToString(), user.PermissionSet.ToString());

            ClearDatabase(user.VisibleName);
        }
    }
}
