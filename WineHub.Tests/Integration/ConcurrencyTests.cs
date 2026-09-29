using Xunit;
using Microsoft.EntityFrameworkCore;
using WineHub.Api.Models;

namespace WineHub.Tests.Integration;

public class ConcurrencyTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task SecondUpdateThrows()
    {
        var name = $"WineHubC_{Guid.NewGuid():N}";
        await using var setup = SqlServerTestDatabase.Create(name);
        try
        {
            await setup.Database.EnsureDeletedAsync();
            await setup.Database.EnsureCreatedAsync();

            setup.Products.Add(new Product { Id = 101, Name = "Barolo", Price = 249 });
            setup.StockItems.Add(new StockItem { ProductId = 101, Quantity = 1 });
            await setup.SaveChangesAsync();

            await using var a = SqlServerTestDatabase.Create(name);
            await using var b = SqlServerTestDatabase.Create(name);
            var x = await a.StockItems.SingleAsync();
            var y = await b.StockItems.SingleAsync();
            x.Quantity = 0;
            await a.SaveChangesAsync();
            y.Quantity = 0;
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => b.SaveChangesAsync());
        }
        finally
        {
            await setup.Database.EnsureDeletedAsync();
        }
    }
}
