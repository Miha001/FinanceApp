using Domain.Resources;
using Microsoft.AspNetCore.Authentication;

namespace Domain.Models;
public class DatabaseTokenAuthOptions : AuthenticationSchemeOptions
{
    public string TokenHeaderName { get; set; } = AuthentificationKeys.UserAuthTokenKey;
}