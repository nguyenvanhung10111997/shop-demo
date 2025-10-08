using example.order.api.Controllers;
using example.order.domain.Entities;
using example.order.infrastructure.Models;
using example.order.service.Features.Orders.DTO;
using example.order.service.Features.Orders.QueryHandlers;
using example.service.Features;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace example.order.api.tests;

[TestFixture]
public class OrdersControllerTests : TestFixtureBase
{
    private OrdersController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _controller = new OrdersController(Mediator);
    }

    [Test]
    public async Task Create_Success()
    {
        Mediator.Send(Arg.Any<CreateOrderCommand>(), Arg.Any<CancellationToken>()).Returns(Unit.Value);

        var request = new CreateOrderCommand
        {
            Description = "Test Order",
            OrderDetails = new List<OrderDetailCreateRequest>
                {
                    new OrderDetailCreateRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductCode = "P001",
                        ProductName = "Product 1",
                        Quantity = 2,
                        Amount = 100m
                    },
                    new OrderDetailCreateRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductCode = "P002",
                        ProductName = "Product 2",
                        Quantity = 1,
                        Amount = 50m
                    }
                }
        };
        var response = await _controller.Create(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();

        var dataResponse = (response as OkObjectResult)?.Value;
        dataResponse.Should().NotBeNull();
    }

    [Test]
    public async Task Search_Success()
    {
        var orderSearchData = new PagingResult<Order>
        {
            TotalRecords = 2,
            Records = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), OrderCode = "ORD001", TotalAmount = 100.0m, TotalQuantity = 1 },
                new Order { Id = Guid.NewGuid(), OrderCode = "ORD002", TotalAmount = 200.0m, TotalQuantity = 2 },
            }
        };

        Mediator.Send(Arg.Any<SearchOrderQuery>(), Arg.Any<CancellationToken>()).Returns(orderSearchData);

        var request = new SearchOrderQuery
        {
            PageNumber = 0,
            PageSize = 10
        };
        var response = await _controller.Search(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();

        var dataResponse = (response as OkObjectResult)?.Value as PagingResult<Order>;
        dataResponse.Should().NotBeNull();
        dataResponse.TotalRecords.Should().Be(2);
        dataResponse.Records.Count().Should().Be(2);
    }

    [Test]
    public async Task GetOrderDetails_Success()
    {
        var orderId = Guid.NewGuid();
        var orderDetails = new List<OrderDetail>
        {
            new OrderDetail { Id = Guid.NewGuid(), OrderId = orderId, ProductId = Guid.NewGuid(), ProductCode = "P001", ProductName = "Product 1", Quantity = 2, Amount = 10.0m },
            new OrderDetail { Id = Guid.NewGuid(), OrderId = orderId, ProductId = Guid.NewGuid(), ProductCode = "P002", ProductName = "Product 2", Quantity = 1, Amount = 20.0m },
            new OrderDetail { Id = Guid.NewGuid(), OrderId = orderId, ProductId = Guid.NewGuid(), ProductCode = "P003", ProductName = "Product 3", Quantity = 5, Amount = 5.0m },
        };

        Mediator.Send(Arg.Any<GetOrderDetailsByIdQuery>(), Arg.Any<CancellationToken>()).Returns(orderDetails);

        var response = await _controller.GetOrderDetails(orderId);

        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();

        var dataResponse = (response as OkObjectResult)?.Value as IEnumerable<OrderDetail>;
        dataResponse.Should().NotBeNull();
        dataResponse.Count().Should().Be(3);
    }
}