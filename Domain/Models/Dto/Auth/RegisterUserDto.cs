namespace Domain.Models.Dto.Auth;
public class RegisterUserDto
{
    public string Name { get; set; }

    public string Password { get; set; }

    public string PasswordConfirm { get; set; }
}