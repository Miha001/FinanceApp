namespace Finances.Domain.Resources;
public class ErrorMessages
{
    public static string RegistrationFailed = "Ошибка регистрации";
    public static string LogoutFailed = "Ошибка выхода из системы";

    public static string UserAlreadyExists = "Такой пользователь уже существует";
    public static string CurrencyAlreadyInFavorites  = "Валюта уже добавлена в избранные";
    public static string PasswordNotEqualsPasswordConfirm = "Пароли не совпадают";
    public static string PasswordIsWrong = "Неверный пароль или пользователь";
    public static string UserNotFound = "Пользователь не найден";

    public static string InvalidClientRequest = "Невалидный запрос";

    public static string InvalidToken = "Невалидный токен";

    public static string AddToFavoriteFailed = "Ошибка добавления курса валют в избранное";
}