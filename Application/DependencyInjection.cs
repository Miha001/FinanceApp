using Microsoft.Extensions.DependencyInjection;

namespace Finances.Application;

/// <summary>
/// Класс для внедрения зависимостей слоя Application
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Внедрение зависимостей слоя Application
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
    }
}