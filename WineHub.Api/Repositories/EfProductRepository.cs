using Microsoft.EntityFrameworkCore;
using WineHub.Api.Data;
using WineHub.Api.Models;

namespace WineHub.Api.Repositories;

public class EfProductRepository(WineHubDbContext db) : IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken ct = default) =>
        await db.Products.AsNoTracking().ToListAsync(ct);

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
}
