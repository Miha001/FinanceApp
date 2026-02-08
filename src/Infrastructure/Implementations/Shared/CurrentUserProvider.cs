using Finances.Application.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Finances.DAL.Implementations.Shared;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public Guid UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("Context user is not available");
            }

            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null || !Guid.TryParse(idClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID claim is missing or invalid");
            }

            return userId;
        }
    }
}
