using WineHub.Api.Requests;

namespace WineHub.Api.Services;

public class OrderValidationService : IOrderValidationService
{
    public void Validate(CreateOrderRequest request)
    {
        if (request.CustomerId <= 0)
            throw new ArgumentException("CustomerId must be greater than zero.");

        if (request.OrderLines.Count == 0)
            throw new ArgumentException("Order must contain at least one line.");

        if (request.OrderLines.Any(x => x.ProductId <= 0 || x.Quantity <= 0))
            throw new ArgumentException("ProductId and quantity must be greater than zero.");
    }
}
