using WineHub.Api.Models;

namespace WineHub.Api.Services;

public interface IPricingService
{
    decimal Calculate(IEnumerable<OrderLine> lines);
}
