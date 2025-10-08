using example.order.domain.Entities;
using example.order.infrastructure.Enums;
using example.service.Features;
using FluentAssertions;

namespace example.order.service.tests.Orders;

[TestFixture]
public class SearchOrderHandlerTests : TestFixtureBase
{
    private SearchOrderHandler _handler = null!;


    [SetUp]
    public void Setup()
    {
        _handler = new SearchOrderHandler(UnitOfWork);
    }

    protected override void SeedData()
    {
        base.SeedData();

        DbContext.Orders.AddRange(new List<Order>
        {
            new Order { Id = Guid.NewGuid(), OrderCode = "ORD001", OrderStatusId = (int)OrderStatusEnum.PENDING, TotalAmount = 100.0m, TotalQuantity = 1 },
            new Order { Id = Guid.NewGuid(), OrderCode = "ORD002", OrderStatusId = (int)OrderStatusEnum.PROCESSING, TotalAmount = 200.0m, TotalQuantity = 2 },
            new Order { Id = Guid.NewGuid(), OrderCode = "ORD003", OrderStatusId = (int)OrderStatusEnum.COMPLETED, TotalAmount = 300.0m, TotalQuantity = 3 },
        });

        DbContext.SaveChanges();
    }

    [Test]
    public async Task SearchOrder_Success()
    {
        var request = new SearchOrderQuery { PageNumber = 0, PageSize = 10 };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Records.Count().Should().Be(3);
        response.TotalRecords.Should().Be(3);
    }

    [Test]
    public async Task SearchOrder_NoContent()
    {
        var request = new SearchOrderQuery { KeySearch = "Invalid", PageNumber = 0, PageSize = 10 };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Records.Should().BeNullOrEmpty();
        response.TotalRecords.Should().Be(0);
    }
}