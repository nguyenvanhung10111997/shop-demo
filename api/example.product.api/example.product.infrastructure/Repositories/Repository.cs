using example.domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace example.infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ExampleDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ExampleDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).CreatedAt = DateTime.UtcNow;
                ((IAuditEntity)entity).UpdatedAt = DateTime.UtcNow;
            }
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                entities.ForEach(x =>
                {
                    ((IAuditEntity)x).CreatedAt = DateTime.UtcNow;
                    ((IAuditEntity)x).UpdatedAt = DateTime.UtcNow;
                });
            }

            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public Task<T> UpdateAsync(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                ((IAuditEntity)entity).UpdatedAt = DateTime.UtcNow;
            }
            _dbSet.Update(entity);

            return Task.FromResult(entity);
        }

        public Task<List<T>> UpdateRangeAsync(List<T> entities)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            {
                entities.ForEach(x =>
                {
                    ((IAuditEntity)x).UpdatedAt = DateTime.UtcNow;
                });
            }
            _dbSet.UpdateRange(entities);

            return Task.FromResult(entities);
        }

        public Task<bool> DeleteAsync(T entity)
        {
            if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            {
                ((IDeleteEntity)entity).IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
                _dbSet.Remove(entity);

            return Task.FromResult(true);
        } 
    }
}
