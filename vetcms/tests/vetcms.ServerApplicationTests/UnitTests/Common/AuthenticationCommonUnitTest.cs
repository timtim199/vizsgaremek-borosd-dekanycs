using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using Xunit;

namespace vetcms.ServerApplicationTests.UnitTests.Common
{
    public class AuthenticationCommonUnitTest
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AuthenticationCommon _authenticationCommon;

        public AuthenticationCommonUnitTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUserRepository = new Mock<IUserRepository>();
            _authenticationCommon = new AuthenticationCommon(_mockConfiguration.Object, _mockUserRepository.Object);
        }

        [Fact]
        public void GenerateAccessToken_ShouldReturnToken()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password"};
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");

            // Act
            var token = _authenticationCommon.GenerateAccessToken(user);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password" };
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(user);

            var token = _authenticationCommon.GenerateAccessToken(user);

            // Act
            var isValid = await _authenticationCommon.ValidateToken(token);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalseForInvalidToken()
        {
            // Arrange
            var token = "invalidtoken";

            // Act
            var isValid = await _authenticationCommon.ValidateToken(token);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalseForExpiredToken()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password" };
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(user);

            var token = _authenticationCommon.GenerateAccessToken(user, DateTime.UtcNow.AddSeconds(-1)); // Expired token

            // Act
            var isValid = await _authenticationCommon.ValidateToken(token);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalseForTokenWithInvalidSignature()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password" };
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(user);

            var token = _authenticationCommon.GenerateAccessToken(user);
            token = token.Substring(0, token.Length - 1) + new Guid().ToString(); // Tamper with the token

            // Act
            var isValid = await _authenticationCommon.ValidateToken(token);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalseForTokenWithInvalidTrackingId()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password" };
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(user);

            var token = _authenticationCommon.GenerateAccessToken(user);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var tamperedToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                jwtToken.Issuer,
                jwtToken.Audiences.FirstOrDefault(),
                jwtToken.Claims.Where(c => c.Type != "tracking-id").Append(new Claim("tracking-id", "invalid-tracking-id")),
                jwtToken.ValidFrom,
                jwtToken.ValidTo,
                jwtToken.SigningCredentials
            ));

            // Act
            var isValid = await _authenticationCommon.ValidateToken(tamperedToken);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalseForTokenWithInvalidPermissionSet()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password" };
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(user);

            var token = _authenticationCommon.GenerateAccessToken(user);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var tamperedToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                jwtToken.Issuer,
                jwtToken.Audiences.FirstOrDefault(),
                jwtToken.Claims.Where(c => c.Type != "permission-set").Append(new Claim("permission-set", "invalid-permission-set")),
                jwtToken.ValidFrom,
                jwtToken.ValidTo,
                jwtToken.SigningCredentials
            ));

            // Act
            var isValid = await _authenticationCommon.ValidateToken(tamperedToken);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalseForNullToken()
        {
            // Arrange
            string token = null;

            // Act & Assert
            Assert.False(await _authenticationCommon.ValidateToken(token));
        }

        [Fact]
        public async Task GetUser_ShouldReturnUserForValidToken()
        {
            // Arrange
            var user = new User { Id = 1, Password = "password"};
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("TAstCtBi3RzzcCiPxHl15gG6uSdBokKatTOcQW48YIkKssbr6x");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _mockConfiguration.Setup(c => c["Jwt:WebAPIAudience"]).Returns("audience");
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(user);

            var token = _authenticationCommon.GenerateAccessToken(user);

            // Act
            var resultUser = await _authenticationCommon.GetUser(token);

            // Assert
            Assert.NotNull(resultUser);
            Assert.Equal(user.Id, resultUser.Id);
        }
    }
}