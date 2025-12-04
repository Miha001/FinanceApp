using Application.Abstractions.Services;
using DAL.Implementations.Services;
using Domain.Constants.Route;
using Domain.Models.Dto;
using Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Users.API.Controllers;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserTokenController(ITokenService tokenService) : ControllerBase
{
    /// <summary>
    /// Обновление токена пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// 
    [Route(Routes.Post.RefreshToken)]
    [HttpPost]
    public async Task<ActionResult<DataResult<TokenDto>>> RefreshToken([FromBody] TokenDto dto)
    {
        var response = await tokenService.RefreshToken(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
