using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Infrastructure.Presistence.Repository
{
    public class FirstTimeAuthenticationCodeRepository : RepositoryBase<FirstTimeAuthenticationCode>, IFirstTimeAuthenticationCodeRepository
    {
        public FirstTimeAuthenticationCodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public string GetCodeByUser(User user)
        {
            var result = Where((u) => u.User == user).Select(u => u.Code).First();
            return result;
        }

        public User GetUserByCode(string code)
        {
            var result = Where(u => u.Code == code).Select(u => u.User).First();
            return result;
        }
    }
}
