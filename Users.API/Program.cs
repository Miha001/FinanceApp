using Infrastructure.Middlewares;
using Finances.Application;
using Finances.Application.Validations;
using Finances.Application.Abstractions.Services;
using Finances.DAL.Implementations.Users;
using Finances.DAL.Implementations.Shared;
using Finances.Application.Abstractions.Validators;
using Finances.Infrastructure.Extensions;
using Finances.Application.Abstractions.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureServices(builder);

builder.Services.AddApplication();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthValidator, AuthValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
