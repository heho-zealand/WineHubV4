namespace WineHub.Api.Requests; public class CreateOrderRequest { public int CustomerId {get;set;} public List<CreateOrderLineRequest> OrderLines {get;set;}=[]; }
