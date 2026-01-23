namespace Finances.Domain.Models.Dto.Auth;

public record RegisterUserDto(string Name, string Password, string PasswordConfirm);