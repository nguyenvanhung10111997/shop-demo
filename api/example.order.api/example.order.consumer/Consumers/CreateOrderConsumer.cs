using example.order.consumer.Services.Interfaces;
using example.order.infrastructure.Messages;
using MassTransit;

namespace example.order.consumer.Consumers;

public class CreateOrderConsumer : IConsumer<OrderCreateMessage>
{
    private readonly ILogger<CreateOrderConsumer> _logger;
    private readonly IOrderService _orderService;

    public CreateOrderConsumer(ILogger<CreateOrderConsumer> logger,
        IOrderService orderService)
    {
        this._logger = logger;
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<OrderCreateMessage> context)
    {
        await _orderService.ProcessOrderAsync(context.Message.OrderTempId);
        _logger.LogInformation("Received message with ID {Id} and timestamp {Timestamp}", context.Message.Identifier, context.Message.Timestamp);
    }
}