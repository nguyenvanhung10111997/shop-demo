using example.domain.Interfaces;
using example.infrastructure;
using example.order.domain.Entities;
using example.order.infrastructure.Messages;
using example.order.infrastructure.Sqs;
using example.order.service.Features.Orders.DTO;
using MediatR;
using Newtonsoft.Json;

namespace example.service.Features
{
    public class CreateOrderHandler : BaseService, IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly ISqsProducer _sqsProducer;
        private readonly IRepository<OrderTemp> _orderTempRepository;

        public CreateOrderHandler(ISqsProducer sqsProducer, Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
        {
            _sqsProducer = sqsProducer;
            _orderTempRepository = unitOfWork.Value.GetRepository<OrderTemp>();
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderTempId = await CreateOrderTemp(request);

            var message = new OrderMessage { OrderTempId = orderTempId };

            await _sqsProducer.SendMessage(message);

            return Unit.Value;
        }

        private async Task<Guid> CreateOrderTemp(CreateOrderCommand request)
        {
            var orderCode = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            var orderTemp = new OrderTemp
            {
                Id = Guid.NewGuid(),
                OrderCode = orderCode,
                Description = request.Description,
                TempData = JsonConvert.SerializeObject(request.OrderDetails)
            };

            await _orderTempRepository.AddAsync(orderTemp);
            await UnitOfWork.SaveChangesAsync();

            return orderTemp.Id;
        }
    }

    public class CreateOrderCommand : IRequest<Unit>
    {
        public string? Description { get; set; }
        public List<OrderDetailCreateRequest> OrderDetails { get; set; } = [];
    }
}
