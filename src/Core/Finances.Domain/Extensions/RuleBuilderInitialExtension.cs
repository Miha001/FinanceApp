using Finances.Domain.Constants.Validation;
using FluentValidation;

namespace Domain.Extensions;

/// <summary>
/// Расширение для IRuleBuilderInitial, в нём вынесены часто повторяющиеся правила валидации.
/// </summary>
public static class RuleBuilderInitialExtension
{
    public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationErrorMessages.PasswordNotEmptyMessage)
            .MinimumLength(ValidationConstraints.PasswordMinimumLength).WithMessage(ValidationErrorMessages.GetPasswordMinimumLengthMessage());
    }

    public static IRuleBuilderOptions<T, string> ValidateUserName<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationErrorMessages.UserNameNotEmptyMessage)
            .MaximumLength(ValidationConstraints.UserNameMaximumLength).WithMessage(ValidationErrorMessages.GetUserNameMaximumLengthMessage());
    }
}