namespace Finances.Domain.Settings;

/// <summary>
/// Настройки Jwt-токена
/// </summary>
public record JwtSettings
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public string Key { get; set; }

    public int ExpirationInMinutes { get; set; }
}