namespace Finances.Domain.Models.Dto.Auth;
public class RegisterUserDto
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Подтверждение пароля
    /// </summary>
    public string PasswordConfirm { get; set; }
}