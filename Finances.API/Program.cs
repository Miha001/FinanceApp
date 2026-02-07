using Finances.Application;
using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.DAL.Implementations.Users;
using Finances.Domain.Settings;
using Finances.Infrastructure.Extensions;
using Infrastructure.Middlewares;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureServices(builder);

builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

builder.Services.AddHttpClient<ICbrClient, CbrClient>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CbrSettings>>().Value;

    client.BaseAddress = new Uri(settings.Url);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();