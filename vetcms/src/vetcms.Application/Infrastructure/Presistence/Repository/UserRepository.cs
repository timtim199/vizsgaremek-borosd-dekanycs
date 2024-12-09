using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Common.Abstractions.Data;
using vetcms.Application.Domain.Entity;

namespace vetcms.Application.Infrastructure.Presistence.Repository
{
    internal class UserRepository : RepositoryBase<User>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public override Task SeedSampleData()
        {
            throw new NotImplementedException();
        }
    }
}
