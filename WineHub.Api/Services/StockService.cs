using WineHub.Api.Models;
using WineHub.Api.Repositories;

namespace WineHub.Api.Services;

public class StockService(IStockRepository repository) : IStockService
{
    public Task<IReadOnlyCollection<StockItem>> GetStockAsync(CancellationToken ct = default) =>
        repository.GetAllAsync(ct);

    public async Task<bool> IsInStockAsync(int id, int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be > 0.");

        var item = await repository.GetByProductIdAsync(id, ct) ?? 
            throw new ArgumentException("Product not in stock.");

        return item.Quantity >= quantity;
    }

    public async Task ReserveStockAsync(int id, int quantity, CancellationToken ct = default)
    {
        var item = await repository.GetByProductIdAsync(id, ct) ?? 
            throw new ArgumentException("Product not in stock.");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be > 0.");

        if (item.Quantity < quantity)
            throw new InvalidOperationException("Not enough stock.");

        item.Quantity -= quantity;
    }

    public async Task ReceiveGoodsAsync(int id, int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be > 0.");

        var item = await repository.GetByProductIdAsync(id, ct) ?? 
            throw new ArgumentException("Product not in stock.");

        item.Quantity += quantity;
    }
}
