using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Constants.Route;
using MediatR;

namespace Finances.API.Endpoints;

public static class CurrenciesEndpoints
{
    public static void MapCurrenciesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/currencies")
                       .RequireAuthorization();

        group.MapGet(Routes.Get.All, GetAll);
        group.MapGet(Routes.Get.Courses, GetRates);
        group.MapPost(Routes.Post.ToFavorites, AddToFavorites);
    }

    private static async Task<IResult> GetAll(IMediator mediator)
    {
        var result = await mediator.Send(new GetAllCurrenciesQuery());

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }

    private static async Task<IResult> GetRates(IMediator mediator)
    {
        var result = await mediator.Send(new GetCurrenciesByUserIdQuery());

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