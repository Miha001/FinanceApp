using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Constants.Route;
using Finances.Domain.Models.Dto.Auth;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Users.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth");

        group.MapPost(Routes.Auth.Register, Register);

        group.MapPost(Routes.Auth.Login, Login);

        group.MapPost(Routes.Auth.Logout, Logout)
             .RequireAuthorization();
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterUserDto dto,
        [FromServices] IValidator<RegisterUserDto> validator,
        [FromServices] IMediator mediator)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var command = new RegisterUserCommand(dto.Name, dto.Password);
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> Login(
        [FromBody] LoginUserDto dto,
        [FromServices] IValidator<LoginUserDto> validator,
        [FromServices] IMediator mediator)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var command = new LoginUserCommand(dto.Name, dto.Password);
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> Logout(
        HttpContext context,
        [FromServices] IMediator mediator,
        [FromServices] ICurrentUserProvider userProvider)
    {
        var token = await context.GetTokenAsync("access_token");

        var command = new LogoutUserCommand(token, userProvider.UserId);
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result);
    }
}