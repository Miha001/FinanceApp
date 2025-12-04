namespace Domain.Enum;
public enum ErrorCodes
{
    // Статус коды для пользователя
    UserNotFound = 0,
    UserAlreadyExists = 2,

    //Статус коды для аутентификации и авторизации
    InvalidClientRequest = 100,
    PasswordIsWrong = 101,
    PasswordNotEqualsPasswordConfirm = 102,
    RegistrationFailed = 103,
    LogoutFailed = 104,

    //Исключительная ситуация
    InternalServerError = 500
}