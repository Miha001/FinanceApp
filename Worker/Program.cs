using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Shared;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Settings;
using Finances.Infrastructure.Db.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using System.Text;
using Worker.Jobs;
using Finances.Application;

var builder = Host.CreateApplicationBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("Default")
                        ?? throw new InvalidOperationException("Строка подключения 'postgres' не найдена.");

builder.Services.Configure<CbrSettings>(
    builder.Configuration.GetSection(nameof(CbrSettings)));

builder.Services.AddApplication();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
builder.Services.AddScoped<IStateSaveChanges, StateSaveChanges>();

builder.Services.AddHttpClient<ICbrClient, CbrClient>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CbrSettings>>().Value;

    client.BaseAddress = new Uri(settings.Url);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("CurrencyUpdateJob");
    q.AddJob<CurrencyUpdateJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey) 
        .WithIdentity("CurrencyUpdateJobTrigger")
        .WithSimpleSchedule(s => s
            .RepeatForever()
            .WithIntervalInMinutes(60)) // Запуск каждые 60 минут //TODO: вынести в конфигурацию
        .StartNow()
    );
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true; 
});

var host = builder.Build();

host.Run();