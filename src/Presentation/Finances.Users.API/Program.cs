using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Application.Abstractions.Users.Commands;
using Finances.DAL.Extensions;
using Finances.DAL.Implementations.Carrencies;
using Finances.DAL.Implementations.Shared;
using Finances.DAL.Implementations.Users;
using Finances.Domain.Abstractions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models.Dto;
using Finances.Domain.Models.Dto.Auth;
using Finances.Domain.Result;
using Finances.Infrastructure.Extensions;
using Finances.Users.API.Endpoints;
using Infrastructure.Middlewares;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureServices(builder);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IStateSaveChanges, StateSaveChanges>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//manual setup of handlers
builder.Services.AddTransient<IRequestHandler<CreateUserCommand, DataResult<User>>,CreateUserCommandHandler>();
builder.Services.AddTransient<IRequestHandler<LoginUserCommand, DataResult<TokenDto>>, LoginUserCommandHandler>();
builder.Services.AddTransient<IRequestHandler<LogoutUserCommand, DataResult<bool>>, LogoutUserCommandHandler>();
builder.Services.AddTransient<IRequestHandler<RegisterUserCommand, DataResult<UserNameDto>>, RegisterUserCommandHandler>();

builder.AddOpenTelemetry(config, "Finances.Users.API");

builder.Services.AddRateLimiter(config);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapAuthEndpoints();
app.Run();