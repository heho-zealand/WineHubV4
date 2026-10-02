using WineHub.Api.Repositories;
using WineHub.Api.Services;

namespace WineHub.Api.Managers;

public class InventoryManager(IStockService stock, IProductService products, IUnitOfWork unitOfWork) : IInventoryManager
{
    public async Task<object[]> GetStockAsync(CancellationToken ct = default)
    {
        var stockItems = await stock.GetStockAsync(ct);
        var result = new List<object>();

        foreach (var item in stockItems)
        {
            var product = await products.GetProductAsync(item.ProductId, ct);
            result.Add(new { productId = item.ProductId, productName = product.Name, quantity = item.Quantity });
        }

        return result.ToArray();
    }

    public async Task ReceiveGoodsAsync(int productId, int quantity, CancellationToken ct = default)
    {
        await stock.ReceiveGoodsAsync(productId, quantity, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}

