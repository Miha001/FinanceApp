namespace Finances.Domain.Constants.Route;

/// <summary>
/// Константы для указания роута в контроллерах.
/// </summary>
public static class Routes
{
    public static class Auth
    {
        public const string Register = nameof(Register);
        public const string Login = nameof(Login);
        public const string Logout = nameof(Logout);
    }

    public static class Get
    {
        public const string Courses = nameof(Courses);
        public const string All = nameof(All);
    }

    public static class Post
    {
        public const string ToFavorites = nameof(ToFavorites);
    }
}