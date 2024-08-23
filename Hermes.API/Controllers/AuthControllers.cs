using System.Security.Claims;
using Hermes.API.Cookies;
using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly CookieManager _cookieManager;

    public AuthController(IUserService userService, CookieManager cookieManager)
    {
        _userService = userService;
        _cookieManager = cookieManager;
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckAuth()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var stringGuid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var guid = new Guid(stringGuid!);

        var user = await _userService.Get(guid);

        return Ok(new { message = "User is authenticated", role, email = user!.Email });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _cookieManager.RemoveAuthorizationCookies(HttpContext);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto dto)
    {
        var user = await _userService.Get(dto.Email, dto.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "User account not found!" });
        }

        await _cookieManager.SetAuthorizationCookies(user, HttpContext);

        return Ok(new { message = "User has logged in successfully", role = user.Role, email = user.Email });
    }
}