using Finances.Application.Abstractions.Shared;
using Finances.Domain.Result;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;

namespace Finances.Application.Abstractions.Users.Commands;

public record LogoutUserCommand([property: JsonIgnore] string Token, Guid UserId) : IRequest<DataResult<bool>>;


public class LogoutUserCommandHandler(ICacheService cacheService)
    : IRequestHandler<LogoutUserCommand, DataResult<bool>>
{
    public async Task<DataResult<bool>> Handle(LogoutUserCommand request, CancellationToken ct)
    {
        // Логика Blacklist
        await cacheService.SetObject(request.Token, request.UserId.ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) }, ct);

        return DataResult<bool>.Success(true);
    }
}