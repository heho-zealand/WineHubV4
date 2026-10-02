using WineHub.Api.Models;

namespace WineHub.Api.Services;

public interface IProductService
{
    Task<Product> GetProductAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyCollection<Product>> GetProductsAsync(CancellationToken ct = default);
}
