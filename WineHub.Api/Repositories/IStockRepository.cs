using WineHub.Api.Models;

namespace WineHub.Api.Repositories;

public interface IStockRepository
{
    Task<IReadOnlyCollection<StockItem>> GetAllAsync(CancellationToken ct = default);
    Task<StockItem?> GetByProductIdAsync(int id, CancellationToken ct = default);
}
