using Finances.Application.Abstractions.Currencies;
using Finances.Application.Abstractions.Currencies.Queries;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Constants.Route;
using Finances.Domain.Models.Dto;
using Finances.Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finances.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CurrenciesController(IMediator mediator) : ControllerBase
{
    [HttpGet(Routes.Get.All)]
    public async Task<ActionResult<CollectionResult<CourseDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllCurrenciesQuery());

        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }

    [HttpGet(Routes.Get.Courses)]
    public async Task<ActionResult<CollectionResult<CourseDto>>> GetRates()
    {
        var result = await mediator.Send(new GetCurrenciesByUserIdQuery());

        if (result.IsSuccess) return Ok(result);

        return BadRequest(result);
    }

    [HttpPost(Routes.Post.ToFavorites)]
    public async Task<ActionResult<CollectionResult<CourseDto>>> AddToFavorites(Guid currencyId)
    {
        var command = new CreateUserCurrencyCommand(currencyId);
        var result = await mediator.Send(command);

        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }
}