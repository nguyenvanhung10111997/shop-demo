using example.service.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace example.product.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [HttpPost(nameof(Search))]
    public async Task<IActionResult> Search(SearchProductQuery obj)
    {
        var result = await _mediator.Send(obj);
        return Ok(result);
    }
}