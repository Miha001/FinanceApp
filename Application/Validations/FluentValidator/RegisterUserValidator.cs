using Domain.Constants.Validation;
using Domain.Extensions;
using Domain.Models.Dto.Auth;
using FluentValidation;

namespace Application.Validations.FluentValidator;
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