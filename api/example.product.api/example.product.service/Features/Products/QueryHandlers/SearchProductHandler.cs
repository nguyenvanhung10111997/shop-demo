using example.domain.Interfaces;
using example.infrastructure;
using example.order.domain.Shared;
using example.order.infrastructure.Models;
using example.product.domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace example.service.Features;

internal class SearchProductHandler : BaseService, IRequestHandler<SearchProductQuery, PagingResult<Product>>
{
    private readonly IReadOnlyRepository<Product> _orderReadOnlyRepository;

    public SearchProductHandler(Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
    {
        _orderReadOnlyRepository = unitOfWork.Value.GetReadOnlyRepository<Product>();
    }

    public async Task<PagingResult<Product>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Product, bool>> condition = x => x.IsDeleted == false
            && (request.CategoryId <= 0 || x.CategoryId == request.CategoryId)
            && (string.IsNullOrEmpty(request.KeySearch) 
                || x.ProductCode == request.KeySearch
                || x.ProductName.Contains(request.KeySearch));

        var entitySort = new EntitySort(nameof(Product.CreatedAt), true);

        var result = await _orderReadOnlyRepository
            .GetPagedAsync(condition, entitySort, request.PageNumber, request.PageSize);

        return new PagingResult<Product>
        {
            TotalRecords = result.totalRecord,
            Records = result.records.AsEnumerable()
        };
    }
}

public class SearchProductQuery : IRequest<PagingResult<Product>>
{
    public string? KeySearch { get; set; }
    public int CategoryId { get; set; }
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}
