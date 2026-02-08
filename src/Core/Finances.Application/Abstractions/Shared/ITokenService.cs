using Finances.Domain.Db.Entities;

namespace Finances.Application.Abstractions.Shared;

/// <summary>
/// Сервис для работы с токенами.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Генерация токена.
    /// </summary>
    /// <param name="user">Текущий пользователь</param>
    /// <returns></returns>
    string Create(User user);
}