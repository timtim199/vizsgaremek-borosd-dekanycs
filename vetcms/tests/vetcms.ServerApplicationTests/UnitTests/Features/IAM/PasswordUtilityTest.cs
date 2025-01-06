using System;
using System.Text;
using Xunit;
using vetcms.ServerApplication.Features.IAM;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplicationTests.UnitTests.Features.IAM
{
    public class PasswordUtilityTest
    {
        [Fact]
        public void GenerateSalt_ShouldReturnSaltOfSpecifiedSize()
        {
            int saltSize = 16;
            byte[] salt = PasswordUtility.GenerateSalt(saltSize);
            Assert.Equal(saltSize, salt.Length);
        }

        [Fact]
        public void HashPassword_ShouldReturnBase64String()
        {
            string password = "TestPassword";
            string hashedPassword = PasswordUtility.HashPassword(password);
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            Assert.NotNull(hashedPasswordBytes);
        }

        [Fact]
        public void CreateUserPassword_ShouldReturnHashedPassword()
        {
            User user = new User();
            string password = "TestPassword";
            string hashedPassword = PasswordUtility.CreateUserPassword(user, password);
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            Assert.NotNull(hashedPasswordBytes);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrueForValidPassword()
        {
            string password = "TestPassword";
            string hashedPassword = PasswordUtility.HashPassword(password);
            bool isValid = PasswordUtility.VerifyPassword(password, hashedPassword);
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
        {
            string password = "TestPassword";
            string wrongPassword = "WrongPassword";
            string hashedPassword = PasswordUtility.HashPassword(password);
            bool isValid = PasswordUtility.VerifyPassword(wrongPassword, hashedPassword);
            Assert.False(isValid);
        }

        [Fact]
        public void GenerateRandomString_ShouldReturnStringOfSpecifiedLength()
        {
            int length = 16;
            string randomString = PasswordUtility.GenerateRandomString(length);
            Assert.Equal(length, randomString.Length);
        }
    }
}