using WineHub.Api.Models;
namespace WineHub.Api.Repositories;
public interface IOrderRepository { Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken ct=default); Task<Order?> GetByIdAsync(int id,CancellationToken ct=default); Task AddAsync(Order order,CancellationToken ct=default); }
public interface ICustomerRepository { Task<IReadOnlyCollection<Customer>> GetAllAsync(CancellationToken ct=default); Task<Customer?> GetByIdAsync(int id,CancellationToken ct=default); }
public interface IProductRepository { Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken ct=default); Task<Product?> GetByIdAsync(int id,CancellationToken ct=default); }
public interface IStockRepository { Task<IReadOnlyCollection<StockItem>> GetAllAsync(CancellationToken ct=default); Task<StockItem?> GetByProductIdAsync(int id,CancellationToken ct=default); }
public interface IUnitOfWork { Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken,Task<T>> operation,CancellationToken ct=default); Task<int> SaveChangesAsync(CancellationToken ct=default); }
