using example.domain.Interfaces;
using example.infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace example.product.service.tests;

public class TestFixtureBase
{
    public Lazy<IUnitOfWork> UnitOfWork { get; set; } = null!;
    public ExampleDbContext DbContext { get; set; } = null!;
    private DbContextOptions<ExampleDbContext> _dbContextOptions = null!;

    public TestFixtureBase()
    {
        var dbName = $"ExampleDb_{DateTime.Now:yyyyMMddHH}";

        _dbContextOptions = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseInMemoryDatabase(dbName)
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        DbContext = new ExampleDbContext(_dbContextOptions);
        UnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(DbContext));
        SeedData();
    }

    protected virtual void SeedData()
    {
    }
}