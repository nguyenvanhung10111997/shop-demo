using example.order.service.Features.Orders.QueryHandlers;
using example.service.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace example.order.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <returns></returns>
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreateOrderCommand obj)
    {
        var result = await _mediator.Send(obj);
        return Ok(result);
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [HttpPost(nameof(Search))]
    public async Task<IActionResult> Search(SearchOrderQuery obj)
    {
        var result = await _mediator.Send(obj);
        return Ok(result);
    }

    /// <summary>
    /// Get order details
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("{orderId}/Details")]
    public async Task<IActionResult> GetOrderDetails(Guid orderId)
    {
        var result = await _mediator.Send(new GetOrderDetailsByIdQuery { OrderId = orderId});
        return Ok(result);
    }
}