using DotNet.Testcontainers.Containers;
using Finances.Infrastructure.Db.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace Finances.UsersIntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("finances_test_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DataContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbConnection));

            if (dbConnectionDescriptor != null)
                services.Remove(dbConnectionDescriptor);

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(_postgres.GetConnectionString());
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        var dummyConfig = new ConfigurationBuilder().Build();

        using var context = new DataContext(options, dummyConfig);
        await context.Database.MigrateAsync();
    }

    public new Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }
}