using Microsoft.EntityFrameworkCore;
using WineHub.Api.Data;
using WineHub.Api.Models;

namespace WineHub.Api.Repositories;

public class EfStockRepository(WineHubDbContext db) : IStockRepository
{
    public async Task<IReadOnlyCollection<StockItem>> GetAllAsync(CancellationToken ct = default) =>
        await db.StockItems.AsNoTracking().ToListAsync(ct);

    public Task<StockItem?> GetByProductIdAsync(int id, CancellationToken ct = default) =>
        db.StockItems.FirstOrDefaultAsync(x => x.ProductId == id, ct);
}
