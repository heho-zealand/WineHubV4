using WineHub.Api.Models;
using WineHub.Api.Repositories;

namespace WineHub.Api.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product> GetProductAsync(int id, CancellationToken ct = default) =>
        await repository.GetByIdAsync(id, ct) ?? throw new ArgumentException($"Product {id} does not exist.");

    public Task<IReadOnlyCollection<Product>> GetProductsAsync(CancellationToken ct = default) =>
        repository.GetAllAsync(ct);
}
