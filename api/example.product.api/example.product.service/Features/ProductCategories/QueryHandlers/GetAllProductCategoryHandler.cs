using example.domain.Interfaces;
using example.infrastructure;
using example.product.domain.Entities;
using MediatR;

namespace example.order.service.Features.ProductCategories.QueryHandlers;

public class GetAllProductCategoryHandler : BaseService, IRequestHandler<GetAllProductCategoryQuery, IEnumerable<ProductCategory>>
{
    private readonly IReadOnlyRepository<ProductCategory> _productCategoryReadOnlyRepository;

    public GetAllProductCategoryHandler(Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
    {
        _productCategoryReadOnlyRepository = unitOfWork.Value.GetReadOnlyRepository<ProductCategory>();
    }

    public async Task<IEnumerable<ProductCategory>> Handle(GetAllProductCategoryQuery request, CancellationToken cancellationToken)
    {
        var orderStatuses = await _productCategoryReadOnlyRepository.GetAllAsync();
        //TODO: Add caching

        return orderStatuses;
    }
}

public class GetAllProductCategoryQuery : IRequest<IEnumerable<ProductCategory>>
{
}