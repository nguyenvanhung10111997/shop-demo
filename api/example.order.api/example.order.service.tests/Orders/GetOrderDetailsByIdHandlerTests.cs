using example.order.domain.Entities;
using example.order.service.Features.Orders.QueryHandlers;
using FluentAssertions;

namespace example.order.service.tests.Orders;

[TestFixture]
public class GetOrderDetailsByIdHandlerTests : TestFixtureBase
{
    private GetOrderDetailsByIdHandler _handler = null!;
    private Guid _orderId = Guid.NewGuid();
    

    [SetUp]
    public void Setup()
    {
        _handler = new GetOrderDetailsByIdHandler(UnitOfWork);
    }

    protected override void SeedData()
    {
        base.SeedData();

        DbContext.OrderDetails.AddRange(new List<OrderDetail>
        {
            new OrderDetail { Id = Guid.NewGuid(), OrderId = _orderId, ProductId = Guid.NewGuid(), ProductCode = "P001", ProductName = "Product 1", Quantity = 2, Amount = 10.0m },
            new OrderDetail { Id = Guid.NewGuid(), OrderId = _orderId, ProductId = Guid.NewGuid(), ProductCode = "P002", ProductName = "Product 2", Quantity = 1, Amount = 20.0m },
            new OrderDetail { Id = Guid.NewGuid(), OrderId = _orderId, ProductId = Guid.NewGuid(), ProductCode = "P003", ProductName = "Product 3", Quantity = 5, Amount = 5.0m },
        });

        DbContext.SaveChanges();
    }

    [Test]
    public async Task GetOrderDetailsById_Success()
    {
        var request = new GetOrderDetailsByIdQuery { OrderId = _orderId };

        var orderStatuses = await _handler.Handle(request, CancellationToken.None);

        orderStatuses.Should().NotBeNull();
        orderStatuses.Count().Should().Be(3);
    }

    [Test]
    public async Task GetOrderDetailsById_NoContent()
    {
        var request = new GetOrderDetailsByIdQuery { OrderId = Guid.NewGuid() };

        var orderStatuses = await _handler.Handle(request, CancellationToken.None);

        orderStatuses.Should().BeNullOrEmpty();
    }
}