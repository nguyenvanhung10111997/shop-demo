using example.domain.Interfaces;
using example.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace example.infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExampleDbContext _context;
        private Dictionary<Type, object> _repositories;
        private Dictionary<Type, object> _readOnlyRepositories;
        private bool _disposedValue;

        public UnitOfWork(ExampleDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _readOnlyRepositories = new Dictionary<Type, object>();
        }

        public async Task<int> SaveChangesAsync()
        {
            var executeResult = 0;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    executeResult = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    executeResult = 0;
                    await transaction.RollbackAsync();
                }
            }

            return executeResult;
        }

        public DbContext GetContext() => _context;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class
        {
            if (_readOnlyRepositories.ContainsKey(typeof(TEntity)))
            {
                return (IReadOnlyRepository<TEntity>)_readOnlyRepositories[typeof(TEntity)];
            }

            var repository = new ReadOnlyRepository<TEntity>(_context);
            _readOnlyRepositories.Add(typeof(TEntity), repository);

            return repository;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork() { Dispose(false); }
    }
}
