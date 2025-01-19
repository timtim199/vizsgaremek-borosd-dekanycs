using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Common.Abstractions.Data
{
    public interface IRepositoryBase<T> where T : AuditedEntity
    {
        Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false);
        Task<T> GetByIdAsync(int id, bool includeDeleted = false);
        IEnumerable<T> Where(Func<T, bool> predicate, bool includeDeleted = false);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
        Task<bool> ExistAsync(int id);
        void LoadReferencedCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
    }
}
