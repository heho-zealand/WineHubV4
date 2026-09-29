using Microsoft.AspNetCore.Mvc;
using WineHub.Api.Services;

namespace WineHub.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(ICustomerService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await service.GetCustomersAsync(ct));
}
