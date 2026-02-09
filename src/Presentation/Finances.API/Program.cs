using Finances.API.Endpoints;
using Finances.Application;
using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Application.Abstractions.Users.Commands;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.DAL.Implementations.Users;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using Finances.Infrastructure.Extensions;
using Infrastructure.Middlewares;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureServices(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IStateSaveChanges, StateSaveChanges>();

builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//manual setup of handlers
builder.Services.AddTransient<IRequestHandler<GetCurrenciesByUserIdQuery, CollectionResult<CourseDto>>, GetCurrenciesByUserIdHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllCurrenciesQuery, CollectionResult<CourseDto>>, GetAllCurrenciesHandler>();
builder.Services.AddTransient<IRequestHandler<CreateUserCurrencyCommand, DataResult<bool>>, CreateUserCurrencyHandler>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapCurrenciesEndpoints();

app.Run();