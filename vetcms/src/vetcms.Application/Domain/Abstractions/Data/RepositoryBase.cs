using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Common.Exceptions;
using vetcms.Application.Domain.Entity;

namespace vetcms.Application.Domain.Abstractions.Data
{
    internal abstract class RepositoryBase<T>(DbContext context) where T : AuditedEntity
    {
        protected readonly DbSet<T> Entities = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await Entities.FindAsync(id);
            if (result == null)
            {
                throw new NotFoundException(nameof(T), id);
            }

            return result;
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            return Entities.Where(predicate);
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
            var entity = await Entities.FindAsync(id);
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

        public abstract Task SeedSampleData();
    }
}
