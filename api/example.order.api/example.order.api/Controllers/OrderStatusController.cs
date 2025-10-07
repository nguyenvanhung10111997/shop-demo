using example.order.service.Features.OrderStatuses.QueryHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace example.order.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderStatusController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderStatusController(IMediator mediator)
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
        var result = await _mediator.Send(new GetAllOrderStatusQuery());
        return Ok(result);
    }
}