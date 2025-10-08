using example.domain.Interfaces;
using example.infrastructure;
using example.order.domain.Entities;
using example.order.infrastructure.Enums;
using example.order.service.Features.OrderStatuses.QueryHandlers;
using FluentAssertions;

namespace example.order.service.tests.OrderStatuses;

[TestFixture]
public class GetAllOrderStatusHandlerTests : TestFixtureBase
{
    private GetAllOrderStatusHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _handler = new GetAllOrderStatusHandler(UnitOfWork);
    }

    protected override void SeedData()
    {
        base.SeedData();

        DbContext.OrderStatus.AddRange(new List<OrderStatus>
        {
            new OrderStatus { Id = 1, StatusCode = OrderStatusEnum.PENDING.ToString(), StatusName = "Pending" },
            new OrderStatus { Id = 2, StatusCode = OrderStatusEnum.PROCESSING.ToString(), StatusName = "PROCESSING" },
        });

        DbContext.SaveChanges();
    }

    [Test]
    public async Task GetAllOrderStatus_Success()
    {
        var request = new GetAllOrderStatusQuery();

        var orderStatuses = await _handler.Handle(request, CancellationToken.None);

        orderStatuses.Should().NotBeNull();
        orderStatuses.Count().Should().Be(2);
    }
}