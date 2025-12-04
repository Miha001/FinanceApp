namespace Domain.Settings;

/// <summary>
/// Настройки Jwt-токена
/// </summary>
public class JwtSettings
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Authority { get; set; }

    public string Key { get; set; }

    public int AccessTokenValidityInDays { get; set; }

    public int RefreshTokenValidityInDays { get; set; }
}