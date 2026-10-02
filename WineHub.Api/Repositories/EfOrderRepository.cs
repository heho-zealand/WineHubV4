using Microsoft.EntityFrameworkCore;
using WineHub.Api.Data;
using WineHub.Api.Models;

namespace WineHub.Api.Repositories;

public class EfOrderRepository(WineHubDbContext db) : IOrderRepository
{
    public async Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken ct = default) =>
        await db.Orders.AsNoTracking().Include(x => x.OrderLines).OrderByDescending(x => x.Id).ToListAsync(ct);

    public Task<Order?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Orders.AsNoTracking().Include(x => x.OrderLines).FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(Order order, CancellationToken ct = default) =>
        db.Orders.AddAsync(order, ct).AsTask();
}
