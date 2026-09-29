using Microsoft.AspNetCore.Mvc;
using WineHub.Api.Services;

namespace WineHub.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await service.GetProductsAsync(ct));
}
