using System.Globalization;
using System.Security.Claims;

namespace Domain.Resources;
public class CustomClaimTypes
{
    public const string UserId = "userId";

    public static IEnumerable<Claim> CreateClaims(string userName, Guid id, DateTime expirationDate)
    {
        return new List<Claim>
        {
            new(UserId, id.ToString()),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Expiration, expirationDate.ToString(CultureInfo.InvariantCulture))
        };
    }
}