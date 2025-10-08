using example.product.domain.Entities;
using example.service.Features;
using FluentAssertions;

namespace example.product.service.tests.Products;
[TestFixture]
public class SearchProductHandlerTests : TestFixtureBase
{
    private SearchProductHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _handler = new SearchProductHandler(UnitOfWork);
    }

    protected override void SeedData()
    {
        base.SeedData();

        DbContext.Products.AddRange(new[]
        {
            new Product
            {
                ProductCode = "P001",
                ProductName = "Laptop",
                CategoryId = 1,
                Price = 1200.00m,
                Rating = 4.5f,
                ImageUrl = "http://example.com/laptop.jpg",
                Stock = 10,
                Description = "High performance laptop",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            },
            new Product
            {
                ProductCode = "P002",
                ProductName = "Smartphone",
                CategoryId = 1,
                Price = 800.00m,
                Rating = 4.7f,
                ImageUrl = "http://example.com/smartphone.jpg",
                Stock = 20,
                Description = "Latest model smartphone",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            },
            new Product
            {
                ProductCode = "P003",
                ProductName = "Book - C# Programming",
                CategoryId = 2,
                Price = 50.00m,
                Rating = 4.9f,
                ImageUrl = "http://example.com/csharpbook.jpg",
                Stock = 100,
                Description = "Comprehensive guide to C# programming",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            }
        });

        DbContext.SaveChanges();
    }

    [Test]
    public async Task SearchOrder_Success()
    {
        var request = new SearchProductQuery { PageNumber = 0, PageSize = 10 };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Records.Count().Should().Be(3);
        response.TotalRecords.Should().Be(3);
    }

    [Test]
    public async Task SearchOrder_NoContent()
    {
        var request = new SearchProductQuery { KeySearch = "Invalid", PageNumber = 0, PageSize = 10 };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Records.Should().BeNullOrEmpty();
        response.TotalRecords.Should().Be(0);
    }
}