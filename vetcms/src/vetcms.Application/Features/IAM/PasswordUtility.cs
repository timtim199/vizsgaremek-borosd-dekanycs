using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Features.IAM
{
    internal static class PasswordUtility
    {
        internal const int SaltSize = 16;

        internal static byte[] GenerateSalt(int size = SaltSize)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[size];
                rng.GetBytes(salt);
                return salt;
            }
        }

        internal static byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string saltedPassword = Convert.ToBase64String(salt) + password;
                byte[] plainSaltedPassword = Encoding.UTF8.GetBytes(saltedPassword);
                byte[] hashedPassword = salt.Concat(sha256.ComputeHash(plainSaltedPassword)).ToArray();

                return hashedPassword;
            }
        }

        internal static byte[] GetSaltFromHashedPassword(byte[] hashedPasswordBytes)
        {
            byte[] salt = hashedPasswordBytes.Take(SaltSize).ToArray();
            return salt;
        }

        public static string HashPassword(string password)
        {
            return Convert.ToBase64String(HashPasswordWithSalt(password, GenerateSalt()));
        }

        public static string CreateUserPassword(User user, string password)
        {
            return HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
                byte[] salt = GetSaltFromHashedPassword(hashedPasswordBytes);

                byte[] inputPasswordHash = HashPasswordWithSalt(password, salt);


                return hashedPasswordBytes.SequenceEqual(inputPasswordHash);
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static string GenerateRandomString(int length = 16)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }
    }
}
