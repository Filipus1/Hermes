using System.Security.Claims;
using AutoMapper;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

    [HttpGet("auth")]
    public IActionResult CheckAuth()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return Ok(new { message = "User is authenticated" });
        }

        return Unauthorized(new { message = "User is not authenticated" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User user = mapper.Map<User>(userDto);

        if (await userService.Get(user.Email) != null)
            return Conflict(new { message = "An account with this email already exists" });

        var status = await userService.Create(user);

        if (!status) return BadRequest(new { message = "Failed to create user" });

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString())
        };

        var authIdentity = new ClaimsIdentity(authClaims, "auth-scheme");
        var principal = new ClaimsPrincipal(authIdentity);

        await HttpContext.SignInAsync("auth-scheme", principal,
        new AuthenticationProperties
        {
            IsPersistent = true
        });

        var activeClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Authentication, "")
        };

        var activeIdentity = new ClaimsIdentity(activeClaims, "active-scheme");
        var activePrincipal = new ClaimsPrincipal(activeIdentity);

        await HttpContext.SignInAsync("active-scheme", activePrincipal,
        new AuthenticationProperties
        {
            IsPersistent = true
        });
        return Ok(new { message = "User has been created successfully" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("auth-scheme");
        await HttpContext.SignOutAsync("active-scheme");

        return Ok(new { message = "User has signed out" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        User user = mapper.Map<User>(userDto);

        var searchedUser = await userService.Get(userDto.Email, userDto.Password);

        if (searchedUser == null) return Unauthorized(new { message = "User account not found!" });

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString())
        };

        var activeClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Anonymous, "")
        };

        var authIdentity = new ClaimsIdentity(authClaims, "auth-scheme");
        var authPrincipal = new ClaimsPrincipal(authIdentity);
        await HttpContext.SignInAsync("auth-scheme", authPrincipal);

        var activeIdentity = new ClaimsIdentity(activeClaims, "active-scheme");
        var activePrincipal = new ClaimsPrincipal(activeIdentity);
        await HttpContext.SignInAsync("active-scheme", activePrincipal);

        return Ok(new { message = "User has logged in successfully" });
    }
}