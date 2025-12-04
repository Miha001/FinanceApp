using Application.Abstractions.Helpers;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.CQRS.Queries;
using Application.Validations.FluentValidator;
using Asp.Versioning;
using DAL.Implementations.Helpers;
using DAL.Implementations.Services;
using Domain.Db.Entities;
using Domain.Settings;
using FluentValidation;
using Infrastructure.Db.Context;
using Infrastructure.Implementations.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Extensions;
public static class CongifurationServicesExtension
{
    /// <summary>
    /// Настройка di сервисов.
    /// </summary>
    public static void ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<DataContext>();
        services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
        services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));

        services.InitEntityRepositories();

        services.AddEndpointsApiExplorer();

        services.AddControllers();

        services.AddAuthenticationAndAuthorization(builder);
        services.AddSwagger();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration)
                         .ReadFrom.Services(services);
        });
        services.InitCaching(builder.Configuration);

        services.InitFluentValidators();
    }

    /// <summary>
    /// Внедрение зависимостей репозиториев для сущностей
    /// </summary>
    /// <param name="services"></param>
    private static void InitEntityRepositories(this IServiceCollection services)
    {
        var types = new List<Type>()
        {
            typeof(UserToken),
            typeof(User),
            typeof(Currency),
            typeof(UserCurrency),
        };

        foreach (var type in types)
        {
            var interfaceType = typeof(IBaseRepository<>).MakeGenericType(type);
            var implementationType = typeof(BaseRepository<>).MakeGenericType(type);
            services.AddScoped(interfaceType, implementationType);
        }
    }
    /// <summary>
    /// Настройка кэширования
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void InitCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();

        if (redisSettings == null || string.IsNullOrEmpty(redisSettings.Url))
        {
            throw new ArgumentNullException("Redis settings are not configured properly.");
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings.Url;
            options.InstanceName = redisSettings.InstanceName ?? string.Empty;
        });
    }

    /// <summary>
    /// Настройка валидации с помощью библиотеки FluentValidation.
    /// </summary>
    /// <param name="services"></param>
    public static void InitFluentValidators(this IServiceCollection services)
    {
        var validatorsTypes = new List<Type>()
        {
            typeof(LoginUserValidator),
            typeof(RegisterUserValidator)
        };

        foreach (var validatorType in validatorsTypes)
        {
            services.AddValidatorsFromAssembly(validatorType.Assembly);
        }
    }

    /// <summary>
    /// Подключение аутентификации и авторизации
    /// </summary>
    /// <param name="services"></param>
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var options = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            var jwtKey = options.Key;
            var issuer = options.Issuer;
            var audience = options.Audience;
            o.Authority = options.Authority;
            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
    }

    /// <summary>
    /// Подключение Swagger
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
    
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });
    }
}