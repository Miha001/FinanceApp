using Domain.Models;
using Domain.Models.Dto;
using Domain.Models.Dto.Auth;
using Domain.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Services;

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
    Task<DataResult<UserVM>> Register(RegisterUserDto dto);

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
