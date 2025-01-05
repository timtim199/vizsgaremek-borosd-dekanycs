using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Common.Abstractions.Data
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        bool HasUserByEmail(string email);
        User GetByEmail(string email);
    }
}
