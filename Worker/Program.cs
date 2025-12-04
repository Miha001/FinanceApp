using Application.Abstractions.Repositories;
using DAL.Implementations.Repositories;
using Infrastructure.Db.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System.Text;
using Worker.Jobs;

var builder = Host.CreateApplicationBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var connectionString = builder.Configuration.GetConnectionString("Default")
                        ?? throw new InvalidOperationException("Строка подключения 'postgres' не найдена.");

builder.Services.AddHttpClient();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

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