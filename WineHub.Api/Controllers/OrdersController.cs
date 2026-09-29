using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineHub.Api.Managers;
using WineHub.Api.Requests;

namespace WineHub.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderManager manager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await manager.GetOrdersAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var order = await manager.GetOrderAsync(id, ct);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateOrderRequest request, CancellationToken ct)
    {
        try
        {
            var order = await manager.CreateOrderAsync(request, ct);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "Concurrent stock update. Retry." });
        }
    }
}
