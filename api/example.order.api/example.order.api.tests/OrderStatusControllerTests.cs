using example.order.api.Controllers;
using example.order.domain.Entities;
using example.order.infrastructure.Enums;
using example.order.service.Features.OrderStatuses.QueryHandlers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace example.order.api.tests;

[TestFixture]
public class OrderStatusControllerTests : TestFixtureBase
{
    private OrderStatusController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _controller = new OrderStatusController(Mediator);
    }

    [Test]
    public async Task GetAllOrderStatus_Success()
    {
        var orderStatuses = new List<OrderStatus>
        {
            new OrderStatus { Id = 1, StatusCode = OrderStatusEnum.PENDING.ToString(), StatusName = "Pending" },
            new OrderStatus { Id = 2, StatusCode = OrderStatusEnum.PROCESSING.ToString(), StatusName = "PROCESSING" },
        };
        Mediator.Send(Arg.Any<GetAllOrderStatusQuery>(), Arg.Any<CancellationToken>()).Returns(orderStatuses);

        var response = await _controller.Get();

        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        
        var dataResponse = (response as OkObjectResult)?.Value as IEnumerable<OrderStatus>;
        dataResponse.Should().NotBeNull();
        dataResponse.Count().Should().Be(2);
    }
}