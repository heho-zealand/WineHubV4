using WineHub.Api.Models;
using WineHub.Api.Requests;

namespace WineHub.Api.Managers;

public interface IOrderManager
{
    Task<IReadOnlyCollection<Order>> GetOrdersAsync(CancellationToken ct = default);
    Task<Order?> GetOrderAsync(int id, CancellationToken ct = default);
    Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default);
}
