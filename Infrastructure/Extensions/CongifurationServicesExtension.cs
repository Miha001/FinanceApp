using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Application.Validations.FluentValidator;
using Finances.DAL.Implementations.Shared;
using Finances.DAL.Implementations.Users;
using Finances.Domain.Db.Entities;
using Finances.Domain.Settings;
using Finances.Infrastructure.Db.Context;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

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
        services.AddScoped<IStateSaveChanges, StateSaveChanges>();

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

                    string? value = await cacheService.GetObjectAsync<string?>(jwt.EncodedToken);

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