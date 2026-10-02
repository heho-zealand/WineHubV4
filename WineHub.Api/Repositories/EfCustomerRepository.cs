using Microsoft.EntityFrameworkCore;
using WineHub.Api.Data;
using WineHub.Api.Models;

namespace WineHub.Api.Repositories;

public class EfCustomerRepository(WineHubDbContext db) : ICustomerRepository
{
    public async Task<IReadOnlyCollection<Customer>> GetAllAsync(CancellationToken ct = default) =>
        await db.Customers.AsNoTracking().ToListAsync(ct);

    public Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
}
