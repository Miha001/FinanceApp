using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Shared;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Db.Entities;
using Finances.Domain.Settings;
using Finances.Infrastructure.Db.Context;
using Finances.Worker.Abstractions;
using Finances.Worker.Currencies.Commands;
using Finances.Worker.Settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using System.Text;
using Worker.Jobs;

var builder = Host.CreateApplicationBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

builder.Services.Configure<CbrSettings>(
    builder.Configuration.GetSection(nameof(CbrSettings)));

var workerSettings = builder.Configuration
    .GetSection(nameof(WorkerSettings))
    .Get<WorkerSettings>() ?? new();

builder.Services.AddHttpClient();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
builder.Services.AddScoped<IStateSaveChanges, StateSaveChanges>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient<IRequestHandler<UpdateCurrencyRatesCommand>, UpdateCurrencyRatesCommandHandler>();

builder.Services.AddHttpClient<ICbrClient, CbrClient>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CbrSettings>>().Value;

    client.BaseAddress = new Uri(settings.Url);

    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds);
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
            .WithIntervalInMinutes(workerSettings.CurrencyUpdateIntervalInMinutes))
        .StartNow()
    );
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

var host = builder.Build();

host.Run();