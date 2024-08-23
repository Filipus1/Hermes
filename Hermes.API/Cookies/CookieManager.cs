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

        var activeClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Anonymous, "")
            };

        var authIdentity = new ClaimsIdentity(authClaims, "auth-scheme");
        var authPrincipal = new ClaimsPrincipal(authIdentity);
        await httpContext.SignInAsync("auth-scheme", authPrincipal);

        var activeIdentity = new ClaimsIdentity(activeClaims, "active-scheme");
        var activePrincipal = new ClaimsPrincipal(activeIdentity);
        await httpContext.SignInAsync("active-scheme", activePrincipal);
    }

    public async Task RemoveAuthorizationCookies(HttpContext httpContext)
    {
        await httpContext.SignOutAsync("auth-scheme");
        await httpContext.SignOutAsync("active-scheme");
    }
}
