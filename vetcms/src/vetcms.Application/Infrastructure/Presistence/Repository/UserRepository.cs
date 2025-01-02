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
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            context.Set<User>().Include(u => u.PasswordResets);
        }

        public bool HasUserByEmail(string email)
            => Where(u => u.Email == email).Any();
                
        public User GetByEmail(string email)
        {
            var result = Where((u) => u.Email == email).First();
            return result;
        }

        public override Task SeedSampleData()
        {
            throw new NotImplementedException();
        }
    }
}
