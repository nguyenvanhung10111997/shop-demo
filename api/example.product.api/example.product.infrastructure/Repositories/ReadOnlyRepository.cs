using example.domain.Interfaces;
using example.order.domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace example.infrastructure.Repositories
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        private readonly ExampleDbContext _context;
        private readonly DbSet<T> _dbSet;

        public ReadOnlyRepository(ExampleDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_dbSet.AsEnumerable());
        }

        public Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(_dbSet.Where(expression));
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<int> ExecuteSqlRawAsync(string query, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public async Task<(int totalRecord, IQueryable<T> records)> GetPagedAsync(Expression<Func<T, bool>>? predicate = null,
            EntitySort entitySort = null, int skip = 0, int take = 10)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (entitySort != null && !string.IsNullOrWhiteSpace(entitySort.SortBy))
            {
                query = OrderByDynamic(query, entitySort.SortBy, entitySort.Descending);
            }

            var totalCount = await query.CountAsync();

            query = query
                .Skip(skip).Take(take);

            return (totalCount, query);
        }

        private IQueryable<T> OrderByDynamic(IQueryable<T> source, string propertyName, bool descending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = descending ? "OrderByDescending" : "OrderBy";

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(result);
        }
    }
}
