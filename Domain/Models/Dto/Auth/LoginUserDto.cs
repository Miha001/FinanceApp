namespace Finances.Domain.Models.Dto.Auth;

/// <summary>
/// Модель данных для авторизации пользователя.
/// </summary>
public class LoginUserDto
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}