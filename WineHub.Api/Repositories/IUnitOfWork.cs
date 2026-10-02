namespace WineHub.Api.Repositories;

public interface IUnitOfWork
{
    Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
