using WineHub.Api.Models; using WineHub.Api.Repositories; using WineHub.Api.Requests;
namespace WineHub.Api.Services;
public interface ICustomerService { Task<Customer> GetCustomerAsync(int id,CancellationToken ct=default); Task<IReadOnlyCollection<Customer>> GetCustomersAsync(CancellationToken ct=default); }
public class CustomerService(ICustomerRepository repo):ICustomerService { public async Task<Customer> GetCustomerAsync(int id,CancellationToken ct=default)=>await repo.GetByIdAsync(id,ct)??throw new ArgumentException($"Customer {id} does not exist."); public Task<IReadOnlyCollection<Customer>> GetCustomersAsync(CancellationToken ct=default)=>repo.GetAllAsync(ct); }
public interface IProductService { Task<Product> GetProductAsync(int id,CancellationToken ct=default); Task<IReadOnlyCollection<Product>> GetProductsAsync(CancellationToken ct=default); }
public class ProductService(IProductRepository repo):IProductService { public async Task<Product> GetProductAsync(int id,CancellationToken ct=default)=>await repo.GetByIdAsync(id,ct)??throw new ArgumentException($"Product {id} does not exist."); public Task<IReadOnlyCollection<Product>> GetProductsAsync(CancellationToken ct=default)=>repo.GetAllAsync(ct); }
public interface IPricingService { decimal Calculate(IEnumerable<OrderLine> lines); }
public class PricingService:IPricingService { public decimal Calculate(IEnumerable<OrderLine> lines)=>decimal.Round(lines.Sum(x=>x.LineTotal),2); }
public interface IOrderValidationService { void Validate(CreateOrderRequest r); }
public class OrderValidationService:IOrderValidationService { public void Validate(CreateOrderRequest r){ if(r.CustomerId<=0)throw new ArgumentException("CustomerId must be greater than zero."); if(r.OrderLines.Count==0)throw new ArgumentException("Order must contain at least one line."); if(r.OrderLines.Any(x=>x.ProductId<=0||x.Quantity<=0))throw new ArgumentException("ProductId and quantity must be greater than zero."); } }
public interface IStockService { Task<IReadOnlyCollection<StockItem>> GetStockAsync(CancellationToken ct=default); Task<bool> IsInStockAsync(int id,int q,CancellationToken ct=default); Task ReserveStockAsync(int id,int q,CancellationToken ct=default); Task ReceiveGoodsAsync(int id,int q,CancellationToken ct=default); }
public class StockService(IStockRepository repo):IStockService {
 public Task<IReadOnlyCollection<StockItem>> GetStockAsync(CancellationToken ct=default)=>repo.GetAllAsync(ct);
 public async Task<bool> IsInStockAsync(int id,int q,CancellationToken ct=default){if(q<=0)throw new ArgumentException("Quantity must be > 0."); var i=await repo.GetByProductIdAsync(id,ct)??throw new ArgumentException("Product not in stock."); return i.Quantity>=q;}
 public async Task ReserveStockAsync(int id,int q,CancellationToken ct=default){var i=await repo.GetByProductIdAsync(id,ct)??throw new ArgumentException("Product not in stock."); if(q<=0)throw new ArgumentException("Quantity must be > 0."); if(i.Quantity<q)throw new InvalidOperationException("Not enough stock."); i.Quantity-=q;}
 public async Task ReceiveGoodsAsync(int id,int q,CancellationToken ct=default){if(q<=0)throw new ArgumentException("Quantity must be > 0."); var i=await repo.GetByProductIdAsync(id,ct)??throw new ArgumentException("Product not in stock."); i.Quantity+=q;}
}
