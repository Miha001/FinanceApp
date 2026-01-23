namespace Finances.Domain.Constants.Validation;

/// <summary>
/// Класс для хранения сообщений об ошибке в валидаторах.
/// </summary>
public static class ValidationErrorMessages
{
    public const string PasswordConfirmNotEqualPasswordMessage = "Пароли не совпадают";
    public const string PasswordConfirmNotEmptyMessage = "Подтвердите пароль";
    public const string PasswordNotEmptyMessage = "Введите пароль";
    public const string UserNameNotEmptyMessage = "Введите имя";

    public static string GetPasswordMinimumLengthMessage() =>
        $"Пароль должен быть не менее {ValidationConstraints.PasswordMinimumLength} символов";

    public static string GetUserNameMaximumLengthMessage() =>
        $"Длина имени должна быть меньше {ValidationConstraints.UserNameMaximumLength} символов";
}