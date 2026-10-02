using WineHub.Api.Models;

namespace WineHub.Api.Services;

public interface IStockService
{
    Task<IReadOnlyCollection<StockItem>> GetStockAsync(CancellationToken ct = default);
    Task<bool> IsInStockAsync(int id, int quantity, CancellationToken ct = default);
    Task ReserveStockAsync(int id, int quantity, CancellationToken ct = default);
    Task ReceiveGoodsAsync(int id, int quantity, CancellationToken ct = default);
}
