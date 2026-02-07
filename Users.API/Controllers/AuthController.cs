using DAL.Controller;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Constants.Route;
using Finances.Domain.Models.Dto.Auth;
using Finances.Domain.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Users.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator,
    IValidator<LoginUserDto> loginValidator,
    IValidator<RegisterUserDto> registerValidator) : BaseApiController
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost(Routes.Auth.Register)]
    public async Task<ActionResult<BaseResult>> Register([FromBody] RegisterUserDto dto)
    {
        var validationResult = await registerValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var command = new RegisterUserCommand(dto.Name, dto.Password);
        var result = await mediator.Send(command);

        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }

    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost(Routes.Auth.Logout)]
    public async Task<IActionResult> Logout()
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        var command = new LogoutUserCommand(token, AuthorizedUserId);
        var result = await mediator.Send(command);

        if (result.IsSuccess) return Ok();
        return BadRequest(result);
    }

    /// <summary>
    /// Логин пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost(Routes.Auth.Login)]
    public async Task<ActionResult<BaseResult>> Login([FromBody] LoginUserDto dto)
    {
        var validationResult = await loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var command = new LoginUserCommand(dto.Name, dto.Password);
        var result = await mediator.Send(command);

        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }
}