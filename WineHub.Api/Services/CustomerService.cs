using WineHub.Api.Models;
using WineHub.Api.Repositories;

namespace WineHub.Api.Services;

public class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<Customer> GetCustomerAsync(int id, CancellationToken ct = default) =>
        await repository.GetByIdAsync(id, ct) ?? throw new ArgumentException($"Customer {id} does not exist.");

    public Task<IReadOnlyCollection<Customer>> GetCustomersAsync(CancellationToken ct = default) =>
        repository.GetAllAsync(ct);
}
