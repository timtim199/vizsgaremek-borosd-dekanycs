using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using Xunit;

namespace vetcms.ServerApplicationTests.IntegrationTests.Common
{
    public class AuthenticationCommonIntegrationTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationCommon _authenticationCommon;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public AuthenticationCommonIntegrationTest()
        {
            var services = new ServiceCollection();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            _authenticationCommon = new AuthenticationCommon(_mockConfiguration.Object, _userRepository);
        }

        [Fact]
        public async Task GenerateAndValidateToken_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var user = new User
            {
                Password = "password",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                VisibleName = "Test User"
            };
            await _userRepository.AddAsync(user);

            // Act
            var token = _authenticationCommon.GenerateAccessToken(user);
            var isValid = await _authenticationCommon.ValidateToken(token);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public async Task GenerateAndValidateToken_ShouldReturnFalseForExpiredToken()
        {
            // Arrange
            var user = new User
            {
                Password = "password",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                VisibleName = "Test User"
            };
            await _userRepository.AddAsync(user);

            // Act
            var token = _authenticationCommon.GenerateAccessToken(user, DateTime.UtcNow.AddSeconds(-1)); // Expired token
            var isValid = await _authenticationCommon.ValidateToken(token);

            // Assert
            Assert.False(isValid);
        }

//        [Fact]
//        public async Task GenerateAndValidateToken_ShouldReturnFalseForTamperedToken()
//        {
//            // Arrange
//            var user = new User
//            {
//                Password = "password",
//                Email = "test@example.com",
//                PhoneNumber = "1234567890",
//                VisibleName = "Test User"
//            };
//            await _userRepository.AddAsync(user);
//
//            // Act
//            var token = _authenticationCommon.GenerateAccessToken(user);
//            token = token.Substring(0, token.Length - 1) + "X"; // Tamper with the token
//            var isValid = await _authenticationCommon.ValidateToken(token);
//
//            // Assert
//            Assert.False(isValid);
//        }

        [Fact]
        public async Task GetUser_ShouldReturnUserForValidToken()
        {
            // Arrange
            var user = new User
            {
                Password = "password",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                VisibleName = "Test User"
            };
            await _userRepository.AddAsync(user);

            // Act
            var token = _authenticationCommon.GenerateAccessToken(user);
            var resultUser = await _authenticationCommon.GetUser(token);

            // Assert
            Assert.NotNull(resultUser);
            Assert.Equal(user.Id, resultUser.Id);
        }
    }
}
