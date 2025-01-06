using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Common.Abstractions.IAM
{
    public interface IAuthenticationCommon
    {
        string GenerateAccessToken(User user, DateTime expirationDate = default);
        Task<bool> ValidateToken(string token);
        Task<User> GetUser(string token);
    }
}
