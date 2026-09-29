using Microsoft.EntityFrameworkCore;
using WineHub.Api.Data;

namespace WineHub.Tests.Integration;

public static class SqlServerTestDatabase
{
    public static WineHubDbContext Create(string databaseName)
    {
        var connectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<WineHubDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new WineHubDbContext(options);
    }
}

