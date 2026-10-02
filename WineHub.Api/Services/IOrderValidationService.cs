using WineHub.Api.Requests;

namespace WineHub.Api.Services;

public interface IOrderValidationService
{
    void Validate(CreateOrderRequest request);
}
