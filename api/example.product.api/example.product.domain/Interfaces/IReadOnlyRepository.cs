using example.order.domain.Shared;
using System.Linq.Expressions;

namespace example.domain.Interfaces
{
    public interface IReadOnlyRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<int> ExecuteSqlRawAsync(string query, params object[] parameters);
        Task<(int totalRecord, IQueryable<T> records)> GetPagedAsync(Expression<Func<T, bool>>? predicate = null,
            EntitySort entitySort = null, int skip = 0, int take = 10);
    }
}
