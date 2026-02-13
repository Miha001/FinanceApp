using Finances.Application.Abstractions.Shared;
using Finances.Application.Validations.FluentValidator;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Settings;
using Finances.Infrastructure.Db.Context;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace Finances.Infrastructure.Extensions;

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

        services.AddEndpointsApiExplorer();

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
    /// Настройка лимита лимита запросов от спама и брутфорса
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddRateLimiter(this IServiceCollection services, IConfiguration configuration) =>
        services.AddRateLimiter(options =>
        {
            var policyConfig = configuration.GetSection("RateLimiting");
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Если юзер залогинен -> лимитируем по ID. Если нет -> по IP.
            options.AddPolicy("fixed-smart", httpContext =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    return GetFixedWindowLimiter(userId, policyConfig);
                }

                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return GetFixedWindowLimiter(ip, policyConfig);
            });
        });

    /// <summary>
    /// Получить RateLimitPartition
    /// </summary>
    /// <param name="partitionKey"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    private static RateLimitPartition<string> GetFixedWindowLimiter(string partitionKey, IConfiguration config)
        => RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = config.GetValue<int>("PermitLimit"),
                            Window = TimeSpan.FromSeconds(config.GetValue<int>("WindowSeconds")),
                            QueueLimit = config.GetValue<int>("QueueLimit"),
                            AutoReplenishment = true
                        });

    /// <summary>
    /// Настройка кэширования
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void InitCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICacheService, CacheService>();

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
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var jwtKey = jwtOptions.Key;
            var issuer = jwtOptions.Issuer;
            var audience = jwtOptions.Audience;

            o.Authority = jwtOptions.Authority;
            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };

        o.Events = new JwtBearerEvents
        {

            OnTokenValidated = async context =>
            {

                if (context.SecurityToken is Microsoft.IdentityModel.JsonWebTokens.JsonWebToken jwt)
                {
                    var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

                    string? value = await cacheService.GetObject<string?>(jwt.EncodedToken);

                    if (value is not null)
                    {
                        context.Fail("Token is invalid");
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                }
            }};
        });

        services.AddAuthorization();
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
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
    
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
    }
}