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
    internal class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return Where((u) => u.Email == email).First();
        }

        public override Task SeedSampleData()
        {
            throw new NotImplementedException();
        }
    }
}
