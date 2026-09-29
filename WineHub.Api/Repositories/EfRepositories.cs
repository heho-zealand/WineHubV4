using Microsoft.EntityFrameworkCore; using WineHub.Api.Data; using WineHub.Api.Models;
namespace WineHub.Api.Repositories;
public class EfOrderRepository(WineHubDbContext db):IOrderRepository {
 public async Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken ct=default)=>await db.Orders.AsNoTracking().Include(x=>x.OrderLines).OrderByDescending(x=>x.Id).ToListAsync(ct);
 public Task<Order?> GetByIdAsync(int id,CancellationToken ct=default)=>db.Orders.AsNoTracking().Include(x=>x.OrderLines).FirstOrDefaultAsync(x=>x.Id==id,ct);
 public Task AddAsync(Order o,CancellationToken ct=default)=>db.Orders.AddAsync(o,ct).AsTask();
}
public class EfCustomerRepository(WineHubDbContext db):ICustomerRepository {
 public async Task<IReadOnlyCollection<Customer>> GetAllAsync(CancellationToken ct=default)=>await db.Customers.AsNoTracking().ToListAsync(ct);
 public Task<Customer?> GetByIdAsync(int id,CancellationToken ct=default)=>db.Customers.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id,ct);
}
public class EfProductRepository(WineHubDbContext db):IProductRepository {
 public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken ct=default)=>await db.Products.AsNoTracking().ToListAsync(ct);
 public Task<Product?> GetByIdAsync(int id,CancellationToken ct=default)=>db.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id,ct);
}
public class EfStockRepository(WineHubDbContext db):IStockRepository {
 public async Task<IReadOnlyCollection<StockItem>> GetAllAsync(CancellationToken ct=default)=>await db.StockItems.AsNoTracking().ToListAsync(ct);
 public Task<StockItem?> GetByProductIdAsync(int id,CancellationToken ct=default)=>db.StockItems.FirstOrDefaultAsync(x=>x.ProductId==id,ct);
}
public class EfUnitOfWork(WineHubDbContext db):IUnitOfWork {
 public Task<int> SaveChangesAsync(CancellationToken ct=default)=>db.SaveChangesAsync(ct);
 public async Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken,Task<T>> op,CancellationToken ct=default){
  await using var tx=await db.Database.BeginTransactionAsync(ct);
  try { var result=await op(ct); await db.SaveChangesAsync(ct); await tx.CommitAsync(ct); return result; }
  catch { await tx.RollbackAsync(ct); throw; }
 }}
