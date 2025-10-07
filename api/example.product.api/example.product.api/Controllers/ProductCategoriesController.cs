using example.order.service.Features.ProductCategories.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace example.product.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetAllProductCategoryQuery());
        return Ok(result);
    }
}