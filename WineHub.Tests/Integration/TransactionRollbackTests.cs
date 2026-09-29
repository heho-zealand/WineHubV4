using Xunit;
using WineHub.Api.Models;

namespace WineHub.Tests.Integration;

public class TransactionRollbackTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task RollbackRestoresStock()
    {
        var name = $"WineHubT_{Guid.NewGuid():N}";
        await using var db = SqlServerTestDatabase.Create(name);
        try
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            db.Products.Add(new Product { Id = 101, Name = "Barolo", Price = 249 });
            db.StockItems.Add(new StockItem { ProductId = 101, Quantity = 12 });
            await db.SaveChangesAsync();

            await using (var tx = await db.Database.BeginTransactionAsync())
            {
                db.StockItems.Single().Quantity = 7;
                await db.SaveChangesAsync();
                await tx.RollbackAsync();
            }

            db.ChangeTracker.Clear();
            Assert.Equal(12, db.StockItems.Single().Quantity);
        }
        finally
        {
            await db.Database.EnsureDeletedAsync();
        }
    }
}

