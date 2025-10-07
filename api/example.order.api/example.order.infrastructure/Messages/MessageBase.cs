namespace example.order.infrastructure.Messages;

public abstract class MessageBase
{
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid Identifier { get; init; } = Guid.NewGuid();
}