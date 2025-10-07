using example.domain.Interfaces;
using example.infrastructure;
using example.order.domain.Entities;
using example.order.domain.Shared;
using example.order.infrastructure.Models;
using MediatR;
using System.Linq.Expressions;

namespace example.service.Features;

internal class SearchOrderHandler : BaseService, IRequestHandler<SearchOrderQuery, PagingResult<Order>>
{
    private readonly IReadOnlyRepository<Order> _orderReadOnlyRepository;

    public SearchOrderHandler(Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
    {
        _orderReadOnlyRepository = unitOfWork.Value.GetReadOnlyRepository<Order>();
    }

    public async Task<PagingResult<Order>> Handle(SearchOrderQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Order, bool>> condition = x => x.IsDeleted == false
            && (request.OrderStatusId <= 0 || x.OrderStatusId == request.OrderStatusId)
            && (string.IsNullOrEmpty(request.KeySearch) || x.OrderCode == request.KeySearch);

        var entitySort = new EntitySort(nameof(Order.CreatedAt), true);

        var result = await _orderReadOnlyRepository
            .GetPagedAsync(condition, entitySort, request.PageNumber, request.PageSize);

        return new PagingResult<Order>
        {
            TotalRecords = result.totalRecord,
            Records = result.records.AsEnumerable()
        };
    }
}

public class SearchOrderQuery : IRequest<PagingResult<Order>>
{
    public string? KeySearch { get; set; }
    public int OrderStatusId { get; set; }
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}
