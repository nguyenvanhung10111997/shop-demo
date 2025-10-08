using example.order.infrastructure.Sqs;
using example.order.service.Features.Orders.DTO;
using example.service.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace example.order.service.tests.Orders;

[TestFixture]
public class CreateOrderHandlerTests : TestFixtureBase
{
    private ISqsProducer _sqsProducer = null!;
    private CreateOrderHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _sqsProducer = Substitute.For<ISqsProducer>();
        _handler = new CreateOrderHandler(_sqsProducer, UnitOfWork);
    }

    [Test]
    public async Task CreateOrder_Success()
    {
        _sqsProducer.SendMessage(Arg.Any<object>()).Returns(Task.CompletedTask);
        var createOrderReq = new CreateOrderCommand
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

        await _handler.Handle(createOrderReq, CancellationToken.None);

        var orderTemp = await DbContext.OrderTemp.FirstOrDefaultAsync();

        orderTemp.Should().NotBeNull();
        orderTemp.Description.Should().Be("Test Order");
        orderTemp.OrderCode.Should().StartWith("ORD-");
        orderTemp.TempData.Should().NotBeNullOrEmpty();
    }
}