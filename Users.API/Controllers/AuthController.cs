using DAL.Controller;
using Finances.Application.Abstractions.Services;
using Finances.Application.Validations.FluentValidator;
using Finances.Domain.Constants.Route;
using Finances.Domain.Models.Dto.Auth;
using Finances.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Users.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService,
    LoginUserValidator loginUserValidator,
    RegisterUserValidator registerUserValidator) : BaseApiController
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost(Routes.Auth.Register)]
    public async Task<ActionResult<BaseResult>> Register([FromBody] RegisterUserDto dto)
    {
        var validationResult = await registerUserValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = await authService.Register(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost(Routes.Auth.Logout)]
    public async Task<IActionResult> Logout()
    {
        var response = await authService.Logout(AuthorizedUserId, HttpContext);

        if (response.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(response);
    }

    /// <summary>
    /// Логин пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost(Routes.Auth.Login)]
    public async Task<ActionResult<BaseResult>> Login([FromBody] LoginUserDto dto)
    {
        var validationResult = await loginUserValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = await authService.Login(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}