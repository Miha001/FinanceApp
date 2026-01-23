using Finances.Domain.Models.Dto;
using Finances.Domain.Models.Dto.Auth;
using Finances.Domain.Result;
using Microsoft.AspNetCore.Http;

namespace Finances.Application.Abstractions.Services;

/// <summary>
/// Сервис авторизации
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<DataResult<UserNameDto>> Register(RegisterUserDto dto);

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<DataResult<TokenDto>> Login(LoginUserDto dto);

    /// <summary>
    /// Выход пользователя из системы(заносим токен в blacklist)
    /// </summary>
    /// <param name="authorizedUserId"></param>
    /// <returns></returns>
    Task<DataResult<bool>?> Logout(Guid authorizedUserId, HttpContext httpContext);
}
