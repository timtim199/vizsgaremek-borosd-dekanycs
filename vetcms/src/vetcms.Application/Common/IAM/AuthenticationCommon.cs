using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using vetcms.ServerApplication.Common.Abstractions.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;

namespace vetcms.ServerApplication.Common.IAM
{
    public class AuthenticationCommon : IAuthenticationCommon
    {

        private readonly IConfiguration configuration;
        private readonly UserRepository userRepository;

        private const string CLAIM_KEY_ID = "id";
        private const string CLAIM_KEY_TRACKING_ID = "tracking-id";
        private const string CLAIM_KEY_PERMISSION_SET = "permission-set";


        public AuthenticationCommon(IConfiguration _config, UserRepository _userRepository)
        {
            configuration = _config;
            userRepository = _userRepository;
        }

        public string GenerateAccessToken(User user)
        {
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
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signing);

            return tokenHandler.WriteToken(token);
        }

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

        private static string CreateTrackingId(string password)
        {
            return CreateMD5(password);
        }

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

        private async Task<User> GetUser(JwtSecurityToken jwtToken)
        {
            int userId = int.Parse(jwtToken.Claims.First(x => x.Type == CLAIM_KEY_ID).Value);
            User user = await userRepository.GetByIdAsync(userId, false);
            return user;
        }

        public async Task<User> GetUser(string token)
        {
            JwtSecurityToken jwtToken = ProccessToken(token);
            return await GetUser(jwtToken);
        }

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
