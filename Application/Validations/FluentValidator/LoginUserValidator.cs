using Domain.Extensions;
using FluentValidation;
using Domain.Models.Dto.Auth;

namespace Application.Validations.FluentValidator;

/// <summary>
/// Валидация авторизации пользователя
/// </summary>
public class LoginUserValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Name).ValidateUserName();

        RuleFor(x => x.Password).ValidatePassword();
    }
}