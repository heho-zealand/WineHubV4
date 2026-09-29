using Microsoft.EntityFrameworkCore;
using WineHub.Api.Data;
using WineHub.Api.Managers;
using WineHub.Api.Repositories;
using WineHub.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
    options.AddPolicy("Vue", policy =>
        policy.WithOrigins("http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddDbContext<WineHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WineHubDatabase")));

// Repositories
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IStockRepository, EfStockRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

// Managers & Services
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<IInventoryManager, InventoryManager>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();
builder.Services.AddScoped<IStockService, StockService>();

var app = builder.Build();

app.UseCors("Vue");
app.MapControllers();
app.Run();

public partial class Program { }

