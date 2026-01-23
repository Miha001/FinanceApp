using Finances.Application;
using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Validators;
using Finances.Application.Validations;
using Finances.DAL.Implementations.Carrencies;
using Finances.Infrastructure.Extensions;
using Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureServices(builder);

builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthValidator, AuthValidator>();
builder.Services.AddScoped<ICurrenciesService, CurrenciesService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();