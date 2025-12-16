using Domain.Db.Entities;
using Domain.Models.Dto;
using Domain.Result;
using System.Security.Claims;

namespace Application.Abstractions.Services;

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