using Xunit;
using WineHub.Api.Models;
using WineHub.Api.Services;

namespace WineHub.Tests;

public class PricingServiceTests
{
    [Fact]
    public void Calculates()
    {
        var service = new PricingService();
        Assert.Equal(1035m, service.Calculate([
            new() { Quantity = 2, UnitPrice = 249m },
            new() { Quantity = 3, UnitPrice = 179m }
        ]));
    }
}
