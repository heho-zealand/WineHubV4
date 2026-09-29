using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineHub.Api.Managers;
using WineHub.Api.Requests;

namespace WineHub.Api.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController(IInventoryManager manager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await manager.GetStockAsync(ct));

    [HttpPost("receive")]
    public async Task<IActionResult> Receive(ReceiveGoodsRequest request, CancellationToken ct)
    {
        try
        {
            await manager.ReceiveGoodsAsync(request.ProductId, request.Quantity, ct);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "Concurrent stock update. Retry." });
        }
    }
}
