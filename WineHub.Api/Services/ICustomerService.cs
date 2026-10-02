using WineHub.Api.Models;

namespace WineHub.Api.Services;

public interface ICustomerService
{
    Task<Customer> GetCustomerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyCollection<Customer>> GetCustomersAsync(CancellationToken ct = default);
}
