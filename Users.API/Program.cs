using Finances.Application;
using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.DAL.Implementations.Users;
using Finances.Infrastructure.Extensions;
using Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureServices(builder);

builder.Services.AddApplication();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IStateSaveChanges, StateSaveChanges>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
