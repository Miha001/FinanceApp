using Application.Abstractions.Services;
using Domain.Constants.Route;
using Domain.Models.VM;
using Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finances.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CurrenciesController(ICurrenciesService currenciesService) : ControllerBase
{
    [HttpGet(Routes.Get.All)]
    public async Task<ActionResult<CollectionResult<CourseVM>>> GetAll()
    {
        var response = await currenciesService.GetAll();

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpGet(Routes.Get.Courses)]
    public async Task<ActionResult<CollectionResult<CourseVM>>> GetRates(Guid userId)
    {
        var response = await currenciesService.GetCoursesByUserId(userId);

        if(response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpGet(Routes.Post.ToFavorites)]
    public async Task<ActionResult<CollectionResult<CourseVM>>> AddToFavorites(Guid userId, Guid currencyId)
    {
        var response = await currenciesService.AddToFavorites(userId, currencyId);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}