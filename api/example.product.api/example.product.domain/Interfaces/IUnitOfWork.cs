using Microsoft.EntityFrameworkCore;

namespace example.domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext GetContext();
        Task<int> SaveChangesAsync();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class;
    }
}
