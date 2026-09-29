using WineHub.Api.Models;
using WineHub.Api.Repositories;
using WineHub.Api.Requests;
using WineHub.Api.Services;

namespace WineHub.Api.Managers;

public class OrderManager(
    IOrderRepository orders,
    ICustomerService customers,
    IProductService products,
    IPricingService pricing,
    IOrderValidationService validation,
    IStockService stock,
    IUnitOfWork uow) : IOrderManager
{
    public Task<IReadOnlyCollection<Order>> GetOrdersAsync(CancellationToken ct = default) =>
        orders.GetAllAsync(ct);

    public Task<Order?> GetOrderAsync(int id, CancellationToken ct = default) =>
        orders.GetByIdAsync(id, ct);

    public Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        validation.Validate(request);
        return uow.ExecuteInTransactionAsync(async transaction =>
        {
            var customer = await customers.GetCustomerAsync(request.CustomerId, transaction);
            var lines = new List<OrderLine>();

            foreach (var item in request.OrderLines)
            {
                var product = await products.GetProductAsync(item.ProductId, transaction);
                lines.Add(new()
                {
                    ProductId = product.Id,
                    ProductNameSnapshot = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            foreach (var line in lines)
            {
                if (!await stock.IsInStockAsync(line.ProductId, line.Quantity, transaction))
                {
                    throw new InvalidOperationException($"Not enough stock for {line.ProductNameSnapshot}.");
                }
            }

            foreach (var line in lines)
            {
                await stock.ReserveStockAsync(line.ProductId, line.Quantity, transaction);
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                CustomerNameSnapshot = customer.Name,
                OrderLines = lines,
                Total = pricing.Calculate(lines),
                Status = "Created",
                CreatedAt = DateTime.UtcNow
            };

            await orders.AddAsync(order, transaction);
            return order;
        }, ct);
    }
}

