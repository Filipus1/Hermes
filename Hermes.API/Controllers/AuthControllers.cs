using System.Security.Claims;
using FluentValidation;
using Hermes.API.Cookies;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly CookieManager _cookieManager;
    private readonly IValidator<UserDto> _authValidator;


    public AuthController(IUserService userService, CookieManager cookieManager, IValidator<UserDto> authValidator)
    {
        _userService = userService;
        _cookieManager = cookieManager;
        _authValidator = authValidator;
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

        var user = await _userService.GetUserbyGuid(guid);

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
        var authValidationResult = await _authValidator.ValidateAsync(dto);

        if (!authValidationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(authValidationResult.ToDictionary()));
        }

        var registeredUser = await _userService.GetUser(dto);

        await _cookieManager.SetAuthorizationCookies(registeredUser!, HttpContext);

        return Ok(new { message = "User has logged in successfully", role = registeredUser!.Role, email = registeredUser.Email });
    }
}