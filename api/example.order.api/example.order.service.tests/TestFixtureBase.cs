using example.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace example.order.service.tests;

public class TestFixtureBase
{
    public ExampleDbContext DbContext { get; set; } = null!;
    private DbContextOptions<ExampleDbContext> _dbContextOptions = null!;

    public TestFixtureBase()
    {
        var dbName = $"ExampleDb_{DateTime.Now:yyyyMMddHH}";

        _dbContextOptions = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        DbContext = new ExampleDbContext(_dbContextOptions);
        SeedData();
    }

    protected virtual void SeedData()
    {
    }
}