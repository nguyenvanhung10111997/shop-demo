namespace example.order.service.Features.Orders.DTO
{
    public class OrderDetailCreateRequest
    {
        public Guid ProductId { get; set; }

        public required string ProductCode { get; set; }

        public required string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}
