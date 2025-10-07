using example.order.domain.Entities;

namespace example.order.infrastructure.Messages;

public class OrderMessage : MessageBase
{
    public required Guid OrderTempId { get; set; }
}