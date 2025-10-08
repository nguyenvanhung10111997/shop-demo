using example.order.consumer.Services;
using example.order.domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace example.order.consumer.tests;
[TestFixture]
public class OrderServiceTests : TestFixtureBase
{
    private OrderService _orderService = null!;
    private Guid _orderTempId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _orderService = new OrderService(UnitOfWork);
    }

    [TearDown]
    public void Cleanup()
    {
        _orderService.Dispose();
    }

    protected override void SeedData()
    {
        base.SeedData();

        var orderDetails = new List<OrderDetail>
        {
            new OrderDetail { Id = Guid.NewGuid(), OrderId = Guid.Empty, ProductId = Guid.NewGuid(), ProductCode = "P001", ProductName = "Product 1", Quantity = 2, Amount = 10.0m },
            new OrderDetail { Id = Guid.NewGuid(), OrderId = Guid.Empty, ProductId = Guid.NewGuid(), ProductCode = "P002", ProductName = "Product 2", Quantity = 1, Amount = 20.0m },
            new OrderDetail { Id = Guid.NewGuid(), OrderId = Guid.Empty, ProductId = Guid.NewGuid(), ProductCode = "P003", ProductName = "Product 3", Quantity = 5, Amount = 5.0m },
        };

        DbContext.OrderTemp.Add(new OrderTemp
        {
            Id = _orderTempId,
            OrderCode = "ORD-20240612123000123",
            Description = "Test Order",
            TempData = JsonConvert.SerializeObject(orderDetails)
        });
        DbContext.SaveChanges();
    }

    [Test]
    public async Task CreateOrder_Success()
    {
        await _orderService.ProcessOrderAsync(_orderTempId);

        var orderTemp = await DbContext.OrderTemp.FirstOrDefaultAsync();

        orderTemp.IsDeleted.Should().BeTrue();

        var order = await DbContext.Orders.FirstOrDefaultAsync();
        var orderDetails = await DbContext.OrderDetails.ToListAsync();

        order.Should().NotBeNull();
        order.OrderCode.Should().Be("ORD-20240612123000123");
        orderDetails.Should().NotBeNullOrEmpty();
        orderDetails.Count.Should().Be(3);
    }
}
