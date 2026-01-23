using Domain.Extensions;
using Finances.Domain.Models.Dto.Auth;
using FluentValidation;

namespace Finances.Application.Validations.FluentValidator;

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