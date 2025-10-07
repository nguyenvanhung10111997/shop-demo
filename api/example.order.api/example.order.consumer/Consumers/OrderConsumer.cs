using example.order.consumer.Services.Interfaces;
using example.order.infrastructure.Messages;
using MassTransit;

namespace example.order.consumer.Consumers;

public class OrderConsumer : IConsumer<OrderMessage>
{
    private readonly ILogger<OrderConsumer> _logger;
    private readonly IOrderService _orderService;

    public OrderConsumer(ILogger<OrderConsumer> logger,
        IOrderService orderService)
    {
        this._logger = logger;
        _orderService = orderService;
    }

    public async Task Consume(ConsumeContext<OrderMessage> context)
    {
        await _orderService.ProcessOrderAsync(context.Message.OrderTempId);
        _logger.LogInformation("Received message with ID {Id} and timestamp {Timestamp}", context.Message.Identifier, context.Message.Timestamp);
    }
}