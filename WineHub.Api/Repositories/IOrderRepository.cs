using WineHub.Api.Models;

namespace WineHub.Api.Repositories;

public interface IOrderRepository
{
    Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken ct = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
}
