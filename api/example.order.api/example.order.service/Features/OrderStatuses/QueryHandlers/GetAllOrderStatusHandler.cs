using example.domain.Interfaces;
using example.infrastructure;
using example.order.domain.Entities;
using MediatR;

namespace example.order.service.Features.OrderStatuses.QueryHandlers;

internal class GetAllOrderStatusHandler : BaseService, IRequestHandler<GetAllOrderStatusQuery, IEnumerable<OrderStatus>>
{
    private readonly IReadOnlyRepository<OrderStatus> _orderStatusReadOnlyRepository;

    public GetAllOrderStatusHandler(Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
    {
        _orderStatusReadOnlyRepository = unitOfWork.Value.GetReadOnlyRepository<OrderStatus>();
    }

    public async Task<IEnumerable<OrderStatus>> Handle(GetAllOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var orderStatuses = await _orderStatusReadOnlyRepository.GetAllAsync();

        return orderStatuses;
    }
}

public class GetAllOrderStatusQuery : IRequest<IEnumerable<OrderStatus>>
{
}