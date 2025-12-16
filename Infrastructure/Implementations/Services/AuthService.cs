using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Abstractions.Validators;
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Application.Resources;
using Domain.Constants;
using Domain.Enum;
using Domain.Models;
using Domain.Models.Dto;
using Domain.Models.Dto.Auth;
using Domain.Result;
using Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ILogger = Serilog.ILogger;

namespace Infrastructure.Implementations.Services;

public class AuthService(IMediator mediator,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IAuthValidator authValidator,
        ICacheService cacheService,
        ILogger logger,
        IOptions<JwtSettings> jwtOptions) : IAuthService
{
    /// <inheritdoc/>
    public async Task<DataResult<TokenDto>> Login(LoginUserDto dto)
    {
        var user = await mediator.Send(new GetUserByNameQuery(dto.Name));
    
        var validateLoginResult = authValidator.ValidateLogin(user, enteredPassword: dto.Password);
        if (!validateLoginResult.IsSuccess)
        {
            return DataResult<TokenDto>.Failure((int)validateLoginResult.Error.Code, validateLoginResult.Error.Message);
        }
    
        var token = tokenService.Create(user);
    
        return DataResult<TokenDto>.Success(new TokenDto()
        {
            JWT = token,
        });
    }

    /// <inheritdoc/>
    public async Task<DataResult<bool>?> Logout(Guid authorizedUserId, HttpContext httpContext)
    {
        try
        {
            var token = await httpContext.GetTokenAsync("access_token");

            //Кладём токен в black-list //TODO: указывать в TTL время жизни токена
            await cacheService.SetObjectAsync(token, authorizedUserId.ToString(),
                new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) });
        }

        catch(Exception e)
        {
            return DataResult<bool>.Failure((int)ErrorCodes.LogoutFailed, ErrorMessages.LogoutFailed);
        }

        return DataResult<bool>.Success(true);
    }

    /// <inheritdoc/>   
    public async Task<DataResult<UserVM>> Register(RegisterUserDto dto)
    {
        var user = await mediator.Send(new GetUserByNameQuery(dto.Name));
    
        var validateRegisterResult = authValidator.ValidateRegister(user, enteredPassword: dto.Password, enteredPasswordConfirm: dto.PasswordConfirm);
        if (!validateRegisterResult.IsSuccess)
        {
            return DataResult<UserVM>.Failure((int)validateRegisterResult.Error.Code, validateRegisterResult.Error.Message);
        }

        try
        {
            user = await mediator.Send(new CreateUserCommand(dto.Name, dto.Password));
    
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
            return DataResult<UserVM>.Failure((int)ErrorCodes.RegistrationFailed, ErrorMessages.RegistrationFailed);
        }

        var newUser = new UserVM
        {
            Name = user.Name,   
        };
        return DataResult<UserVM>.Success(newUser);
    }
}