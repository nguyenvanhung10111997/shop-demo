using example.domain.Interfaces;
using example.infrastructure;
using example.order.domain.Entities;
using MediatR;

namespace example.order.service.Features.Orders.QueryHandlers;

internal class GetOrderDetailsByIdHandler : BaseService, IRequestHandler<GetOrderDetailsByIdQuery, IEnumerable<OrderDetail>>
{
    private readonly IReadOnlyRepository<OrderDetail> _orderDetailReadOnlyRepository;

    public GetOrderDetailsByIdHandler(Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
    {
        _orderDetailReadOnlyRepository = unitOfWork.Value.GetReadOnlyRepository<OrderDetail>();
    }

    public async Task<IEnumerable<OrderDetail>> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var orderDetails = await _orderDetailReadOnlyRepository
            .GetAsync(od => !od.IsDeleted && od.OrderId == request.OrderId);

        return orderDetails;
    }
}

public class GetOrderDetailsByIdQuery : IRequest<IEnumerable<OrderDetail>>
{
    public Guid OrderId { get; set; }
}
