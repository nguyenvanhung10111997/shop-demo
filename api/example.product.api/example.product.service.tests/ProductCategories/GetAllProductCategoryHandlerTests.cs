using example.order.service.Features.ProductCategories.QueryHandlers;
using example.product.domain.Entities;
using FluentAssertions;

namespace example.product.service.tests.ProductCategories;

[TestFixture]
public class GetAllProductCategoryHandlerTests : TestFixtureBase
{
    private GetAllProductCategoryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _handler = new GetAllProductCategoryHandler(UnitOfWork);
    }

    protected override void SeedData()
    {
        base.SeedData();

        DbContext.ProductCategories.AddRange(new[]
        {
            new ProductCategory
            {
                CategoryName = "Electronics",
                Description = "Electronic gadgets and devices",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            },
            new ProductCategory
            {
                CategoryName = "Books",
                Description = "Various kinds of books",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSetup"
            }
        });

        DbContext.SaveChanges();
    }

    [Test]
    public async Task GetAllOrderStatus_Success()
    {
        var request = new GetAllProductCategoryQuery();

        var orderStatuses = await _handler.Handle(request, CancellationToken.None);

        orderStatuses.Should().NotBeNull();
        orderStatuses.Count().Should().Be(2);
    }
}