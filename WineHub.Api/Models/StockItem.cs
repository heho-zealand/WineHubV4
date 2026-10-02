namespace WineHub.Api.Models;

public class StockItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
