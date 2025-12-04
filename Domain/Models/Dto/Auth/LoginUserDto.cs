using System.Security;

namespace Domain.Models.Dto.Auth;

/// <summary>
/// Модель данных для авторизации пользователя.
/// </summary>
public class LoginUserDto
{
    public string Name { get; set; }

    public string Password { get; set; }
}
