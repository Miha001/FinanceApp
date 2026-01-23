using Domain.Extensions;
using Finances.Domain.Constants.Validation;
using Finances.Domain.Models.Dto.Auth;
using FluentValidation;

namespace Finances.Application.Validations.FluentValidator;
public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name).ValidateUserName();
        RuleFor(x => x.Password).ValidatePassword();

        RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage(ValidationErrorMessages.PasswordConfirmNotEmptyMessage)
                .Equal(x => x.Password).WithMessage(ValidationErrorMessages.PasswordConfirmNotEqualPasswordMessage);
    }
}