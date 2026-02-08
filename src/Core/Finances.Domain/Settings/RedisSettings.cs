namespace Finances.Domain.Settings;

/// <summary>
/// Настройки для redis
/// </summary>
public class RedisSettings
{
    /// <summary>
    /// Url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Название экземпляра
    /// </summary>
    public string InstanceName { get; set; }
}