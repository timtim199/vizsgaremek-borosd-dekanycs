using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;

namespace vetcms.ServerApplication.Common.IAM
{
    /// <summary>
    /// Az AuthenticationCommon osztály JWT tokenek generálására és érvényesítésére, valamint a tokenekből származó felhasználói információk lekérésére szolgáló metódusokat biztosít.
    /// Az <see cref="IAuthenticationCommon"/> interfészt valósítja meg.
    /// </summary>
    public class AuthenticationCommon : IAuthenticationCommon
    {
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        private const string CLAIM_KEY_ID = "id";
        private const string CLAIM_KEY_TRACKING_ID = "tracking-id";
        private const string CLAIM_KEY_PERMISSION_SET = "permission-set";

        /// <summary>
        /// Inicializál egy új példányt az <see cref="AuthenticationCommon"/> osztályból.
        /// </summary>
        /// <param name="_config">A konfigurációs beállítások.</param>
        /// <param name="_userRepository">A felhasználói adatok elérésére szolgáló felhasználói adattár.</param>
        public AuthenticationCommon(IConfiguration _config, IUserRepository _userRepository)
        {
            configuration = _config;
            userRepository = _userRepository;
        }

        /// <summary>
        /// JWT tokent generál egy adott felhasználó számára opcionális lejárati dátummal.
        /// </summary>
        /// <param name="user">A felhasználó, akinek a token generálva lesz.</param>
        /// <param name="expirationDate">A token opcionális lejárati dátuma. Alapértelmezés szerint 7 nap, ha nincs megadva.</param>
        /// <returns>A generált JWT token string formátumban.</returns>
        public string GenerateAccessToken(User user, DateTime expirationDate = default)
        {
            if (expirationDate == default)
            {
                expirationDate = DateTime.Now.AddDays(7);
            }
            string audience = configuration["Jwt:WebAPIAudience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var claims = new[] {
                    new Claim(CLAIM_KEY_ID, user.Id.ToString()),
                    new Claim(CLAIM_KEY_TRACKING_ID, CreateTrackingId(user.Password)),
                    new Claim(CLAIM_KEY_PERMISSION_SET, user.PermissionSet)
                };

            var signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                audience,
                claims,
                expires: expirationDate,
                signingCredentials: signing);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Érvényesíti a megadott JWT tokent.
        /// </summary>
        /// <param name="token">Az érvényesítendő JWT token.</param>
        /// <returns>Boolean érték, amely jelzi, hogy a token érvényes-e vagy sem.</returns>
        public async Task<bool> ValidateToken(string token)
        {
            try
            {
                JwtSecurityToken jwtToken = ProccessToken(token);

                User user = await GetUser(jwtToken);
                string trackingId = jwtToken.Claims.First(x => x.Type == CLAIM_KEY_TRACKING_ID).Value;
                string permissionSet = jwtToken.Claims.First(x => x.Type == CLAIM_KEY_PERMISSION_SET).Value;

                if(CreateTrackingId(user.Password) != trackingId || user.PermissionSet != permissionSet)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // return null if validation fails
                return false;
            }
        }

        /// <summary>
        /// Lekéri a megadott JWT tokenhez társított felhasználót.
        /// </summary>
        /// <param name="token">A JWT token, amelyből a felhasználó lekérhető.</param>
        /// <returns>A megadott JWT tokenhez társított felhasználó.</returns>
        public async Task<User> GetUser(string token)
        {
            JwtSecurityToken jwtToken = ProccessToken(token);
            return await GetUser(jwtToken);
        }

        /// <summary>
        /// Generál egy követési azonosítót a felhasználó jelszava alapján az MD5 algoritmus használatával.
        /// </summary>
        /// <param name="password">A felhasználó jelszava.</param>
        /// <returns>A generált követési azonosító.</returns>
        private static string CreateTrackingId(string password)
        {
            return CreateMD5(password);
        }

        /// <summary>
        /// Kiszámítja egy adott bemeneti string MD5 hash értékét.
        /// </summary>
        /// <param name="input">A hash-elendő bemeneti string.</param>
        /// <returns>A bemeneti string MD5 hash értéke.</returns>
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }

        /// <summary>
        /// Lekéri a megadott JWT tokenhez társított felhasználót azáltal, hogy kinyeri a felhasználói azonosítót a tokenből, és lekéri a felhasználót az adattárból.
        /// </summary>
        /// <param name="jwtToken">A JWT token, amelyből a felhasználói azonosítót kinyerjük.</param>
        /// <returns>A megadott JWT tokenhez társított felhasználó.</returns>
        private async Task<User> GetUser(JwtSecurityToken jwtToken)
        {
            int userId = int.Parse(jwtToken.Claims.First(x => x.Type == CLAIM_KEY_ID).Value);
            User user = await userRepository.GetByIdAsync(userId, false);
            return user;
        }

        /// <summary>
        /// Feldolgoz egy adott JWT tokent azáltal, hogy érvényesíti és kinyeri a követeléseket.
        /// </summary>
        /// <param name="token">A feldolgozandó JWT token.</param>
        /// <returns>A feldolgozott JWT token.</returns>
        /// <exception cref="ArgumentNullException">Akkor dobódik, ha a token null.</exception>
        private JwtSecurityToken ProccessToken(string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            string audience = configuration["Jwt:WebAPIAudience"];

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuers = new List<string>()
                {
                    configuration["Jwt:Issuer"]
                },

                ValidateAudience = true,
                ValidAudiences = new List<string>()
                {
                    audience
                },

                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken;
        }
    }
}
