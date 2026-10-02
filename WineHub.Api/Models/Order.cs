namespace WineHub.Api.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerNameSnapshot { get; set; } = string.Empty;
    public List<OrderLine> OrderLines { get; set; } = [];
    public decimal Total { get; set; }
    public string Status { get; set; } = "Created";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
