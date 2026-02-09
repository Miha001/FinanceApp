using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Constants.Route;
using Finances.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finances.API.Endpoints;

public static class CurrenciesEndpoints
{
    public static void MapCurrenciesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/currencies")
                       .RequireAuthorization();

        group.MapGet(Routes.Get.All, GetAll);
        group.MapGet(Routes.Get.Courses, GetCourses);
        group.MapPost(Routes.Post.ToFavorites, AddToFavorites);
    }

    private static async Task<IResult> GetAll([AsParameters] Pagination pagination, IMediator mediator)
    {
        var result = await mediator.Send(new GetAllCurrenciesQuery(pagination));

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> GetCourses([AsParameters] Pagination pagination, IMediator mediator)
    {
        var result = await mediator.Send(new GetCurrenciesByUserIdQuery(pagination));

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> AddToFavorites(Guid currencyId, IMediator mediator)
    {
        var command = new CreateUserCurrencyCommand(currencyId);
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}