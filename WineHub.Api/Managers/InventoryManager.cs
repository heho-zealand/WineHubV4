using WineHub.Api.Repositories;
using WineHub.Api.Services;
namespace WineHub.Api.Managers;

public interface IInventoryManager { Task<object[]> GetStockAsync(CancellationToken ct = default); Task ReceiveGoodsAsync(int id, int q, CancellationToken ct = default); }
public class InventoryManager(IStockService stock, IProductService products, IUnitOfWork uow) : IInventoryManager
{
    public async Task<object[]> GetStockAsync(CancellationToken ct = default) 
    { var s = await stock.GetStockAsync(ct); 
        var list = new List<object>(); 
        foreach (var i in s) 
        { var p = await products.GetProductAsync(i.ProductId, ct); 
            list.Add(new { productId = i.ProductId, productName = p.Name, quantity = i.Quantity });
        } 
        return list.ToArray(); 
    }
    public async Task ReceiveGoodsAsync(int id, int q, CancellationToken ct = default) { await stock.ReceiveGoodsAsync(id, q, ct); await uow.SaveChangesAsync(ct); }
}
