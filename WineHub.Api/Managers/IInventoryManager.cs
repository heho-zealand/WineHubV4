namespace WineHub.Api.Managers;

public interface IInventoryManager
{
    Task<object[]> GetStockAsync(CancellationToken ct = default);
    Task ReceiveGoodsAsync(int productId, int quantity, CancellationToken ct = default);
}
