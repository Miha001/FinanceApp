using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var conString = configuration.GetConnectionString("Default");

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<IConfiguration>(configuration)
    .AddDbContext<DataContext>()
    .BuildServiceProvider();

var context = serviceProvider.GetService<DataContext>();
var migrator = context.GetInfrastructure().GetService<IMigrator>();

var targetMigration = string.Empty;

var getNameTask = () => { targetMigration = Console.ReadLine(); };

Task.Run(getNameTask);

Console.WriteLine("Введите название целевой миграции или оставьте пустую строку, чтобы обновить всё, ожидание 5 секунд.");
var delay = Task.Delay(TimeSpan.FromSeconds(5));
var seonds = 0;
while(!delay.IsCompleted)
{
    //Отсчёт секунд
    seonds++;
    Thread.Sleep(TimeSpan.FromSeconds(1));
    Console.WriteLine(seonds);
}

try
{
    if(!string.IsNullOrEmpty(targetMigration))
    {
        migrator.Migrate(targetMigration);
    }
    else
    {
        migrator.Migrate();
    }
}
catch(Exception ex)
{

    Console.WriteLine($"Ошибка запуска миграции: {targetMigration}:\n{ex.Message}\n{ex.StackTrace}");
    throw;
}

Console.WriteLine("Завершение.");