using MassTransit;
using Microsoft.Extensions.Logging;

namespace example.order.infrastructure.Sqs;

internal class SqsProducer : ISqsProducer
{
    private readonly IBus _messageBus;
    private readonly ILogger<SqsProducer> _logger;

    public SqsProducer(IBus messageBus, ILogger<SqsProducer> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task SendMessage<T>(T message)
    {
        await _messageBus.Publish(message);
        _logger.LogInformation("Sent message {Timestamp}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
    }
}