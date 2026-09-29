using Microsoft.EntityFrameworkCore;
using WineHub.Api.Models;

namespace WineHub.Api.Data;

public class WineHubDbContext(DbContextOptions<WineHubDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<StockItem> StockItems => Set<StockItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Product configuration - allow explicit IDs
        builder.Entity<Product>().Property(x => x.Id).ValueGeneratedNever();

        // Customer configuration - allow explicit IDs
        builder.Entity<Customer>().Property(x => x.Id).ValueGeneratedNever();

        // Stock configuration with concurrency control
        builder.Entity<StockItem>().HasKey(x => x.ProductId);
        builder.Entity<StockItem>().Property(x => x.RowVersion).IsRowVersion();

        // Order relationships
        builder.Entity<Order>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderLine relationships
        builder.Entity<OrderLine>()
            .HasOne<Order>()
            .WithMany(x => x.OrderLines)
            .HasForeignKey(x => x.OrderId);

        builder.Entity<OrderLine>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Stock to Product relationship
        builder.Entity<StockItem>()
            .HasOne<Product>()
            .WithOne()
            .HasForeignKey<StockItem>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Decimal precision for prices
        builder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);
        builder.Entity<Order>().Property(x => x.Total).HasPrecision(18, 2);
        builder.Entity<OrderLine>().Property(x => x.UnitPrice).HasPrecision(18, 2);
    }
}


