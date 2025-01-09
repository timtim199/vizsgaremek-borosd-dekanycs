using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Exceptions;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Common.Abstractions.Data
{
    public abstract class RepositoryBase<T>(DbContext context) : IRepositoryBase<T>
        where T : AuditedEntity
    {
        protected readonly DbSet<T> Entities = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false)
        {
            return await Entities.Where(e => includeDeleted || !e.Deleted).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, bool includeDeleted = false)
        {

            var result = await Entities.Where(e => (includeDeleted || !e.Deleted) && e.Id == id).FirstAsync();
            if (result == null)
            {
                throw new NotFoundException(nameof(T), id);
            }

            return result;
        }

        public IEnumerable<T> Where(Func<T, bool> predicate, bool includeDeleted = false)
        {
            return Entities.Where(predicate).Where(e => includeDeleted || !e.Deleted);
        }

        public async Task<T> AddAsync(T entity)
        {
            await Entities.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Entities.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await Entities.Where(e => !e.Deleted && e.Id == id).FirstAsync();
            if (entity == null)
            {
                throw new NotFoundException(nameof(T), id);
            }

            Entities.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistAsync(int id)
        {


            return await Entities.AnyAsync(e => e.Id == id);
        }

        public void LoadReferencedCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
            where TProperty : class
        {
            Entities.Entry(entity).Collection(propertyExpression).Load();
        }
    }
}
