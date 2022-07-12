using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using OrderMgmt.Application.Models;
using OrderMgmt.Application.Queries;

namespace OrderMgmt.API.Controllers.v1;

/// <summary>
/// Orders endpoints
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class OrdersController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        ICurrentUserService currentUserService,
        IMediator mediator,
        ILogger<OrdersController> logger)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
        _logger = logger;
    }
    
    [Authorize(Policy = "users")]
    [ProducesResponseType(typeof(IEnumerable<OrderSummary>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<OkObjectResult> GetOrdersAsync()
    {
        _logger.LogInformation("--> Executing Query: GetOrders");
        
        var userId = _currentUserService.UserId;
        var getOrdersQuery = new GetOrdersFromUserQuery(userId);

        var orders = await _mediator.Send(getOrdersQuery);

        return Ok(orders);
    }

    [Authorize(Policy = "users")]
    [ProducesResponseType(typeof(Order), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpGet]
    [Route("{orderId:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<OkObjectResult> GetOrderAsync(Guid orderId)
    {
        _logger.LogInformation("--> Executing Query: GetOrder");
        
        var getOrderQuery = new GetOrderQuery(orderId);

        var order = await _mediator.Send(getOrderQuery);

        return Ok(order);
    }
}
