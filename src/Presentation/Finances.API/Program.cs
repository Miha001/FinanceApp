using Finances.API.Endpoints;
using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Application.Behaviors;
using Finances.DAL.Extensions;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.DAL.Implementations.Users;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using Finances.Infrastructure.Extensions;
using Infrastructure.Middlewares;
using MediatR;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.ConfigureServices(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAuditLogService, MongoAuditLogService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IStateSaveChanges, StateSaveChanges>();

builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    cfg.AddOpenBehavior(typeof(AuditLogBehavior<,>));
});

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connString = config.GetConnectionString("MongoDb");
    return new MongoClient(connString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var databaseName = config["MongoDb:DatabaseName"];
    return client.GetDatabase(databaseName);
});

//manual setup of handlers
builder.Services.AddTransient<IRequestHandler<GetCurrenciesByUserIdQuery, CollectionResult<CourseDto>>, GetCurrenciesByUserIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllCurrenciesQuery, CollectionResult<CourseDto>>, GetAllCurrenciesHandler>();
builder.Services.AddTransient<IRequestHandler<CreateUserCurrencyCommand, DataResult<bool>>, CreateUserCurrencyHandler>();

builder.AddOpenTelemetry(config, "Finances.API");

builder.Services.AddRateLimiter(config);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapCurrenciesEndpoints();

app.UseRateLimiter();

app.Run();