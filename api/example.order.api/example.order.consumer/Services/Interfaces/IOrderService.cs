namespace example.order.consumer.Services.Interfaces
{
    public interface IOrderService : IDisposable
    {
        Task ProcessOrderAsync(Guid orderTempId);
    }
}
