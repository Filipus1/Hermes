using System.Security.Claims;
using Hermes.Application.Entities;
using Microsoft.AspNetCore.Authentication;

namespace Hermes.API.Cookies;
public class CookieManager
{
    public async Task SetAuthorizationCookies(User user, HttpContext httpContext)
    {
        var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

        var authIdentity = new ClaimsIdentity(authClaims, "auth-scheme");
        var authPrincipal = new ClaimsPrincipal(authIdentity);
        await httpContext.SignInAsync("auth-scheme", authPrincipal);
    }

    public async Task RemoveAuthorizationCookies(HttpContext httpContext)
    {
        await httpContext.SignOutAsync("auth-scheme");
    }
}
