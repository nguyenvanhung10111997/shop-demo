using example.order.domain.Entities;

namespace example.order.infrastructure.Messages;

public class OrderCreateMessage : MessageBase
{
    public required Guid OrderTempId { get; set; }
}