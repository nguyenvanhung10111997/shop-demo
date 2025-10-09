namespace example.order.infrastructure.Messages;

public class OrderUpdateMessage : MessageBase
{
    public required Guid OrderId { get; set; }
}