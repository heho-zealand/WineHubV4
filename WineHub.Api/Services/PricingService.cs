using WineHub.Api.Models;

namespace WineHub.Api.Services;

public class PricingService : IPricingService
{
    public decimal Calculate(IEnumerable<OrderLine> lines) =>
        decimal.Round(lines.Sum(x => x.LineTotal), 2);
}
