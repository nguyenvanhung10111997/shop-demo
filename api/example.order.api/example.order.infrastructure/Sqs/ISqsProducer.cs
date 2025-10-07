namespace example.order.infrastructure.Sqs
{
    public interface ISqsProducer
    {
        Task SendMessage<T>(T message);
    }
}
