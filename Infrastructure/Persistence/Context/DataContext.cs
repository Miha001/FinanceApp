using Domain.Db.Context.EntityTypeConfigurations;
using Finances.DAL.Db.Context.EntityTypeConfigurations;
using Finances.Domain.Db.Context.EntityTypeConfigurations;
using Finances.Domain.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Finances.Infrastructure.Db.Context;

/// <summary>
/// Контекст БД Postgres
/// </summary>
/// <remarks>
/// Для миграций используй следующие команды Package Manager Console:
/// <br/>Создать: dotnet ef migrations add NAME_MIGRATION --project Infrastructure --startup-project Users.API --context DataContext
/// <br/>Удалить:  dotnet ef migrations remove --project Infrastructure --startup-project Users.API --context DataContext
/// <br/>Применить: dotnet ef database update --project Infrastructure --startup-project Users.API --context DataContext
/// </remarks>
public class DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<UserCurrency> UserCurrencies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"), b => b.EnableRetryOnFailure()
        .MigrationsAssembly(typeof(DataContext).Assembly.FullName));

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder
        .ApplyConfiguration(new UserConfiguration())
        .ApplyConfiguration(new CurrencyConfiguration())
        .ApplyConfiguration(new UserCurrencyConfiguration());
}