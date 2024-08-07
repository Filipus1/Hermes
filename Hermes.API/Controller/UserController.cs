using System.Security.Claims;
using AutoMapper;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> CheckAuth()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return Unauthorized(new { message = "User is not authenticated" });
        }

        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var stringGuid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var guid = new Guid(stringGuid!);

        var user = await userService.Get(guid);

        return Ok(new { message = "User is authenticated", role, email = user!.Email });
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

        return Ok(new { message = "User has been created successfully" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("auth-scheme");
        await HttpContext.SignOutAsync("active-scheme");

        return Ok(new { message = "User has signed out" });
    }

    [Authorize(Roles = "admin")]
    [HttpGet("collaborators")]
    public async Task<IActionResult> GetCollaborators()
    {
        var users = await userService.GetAll();
        var collaborators = users
            .Where(u => u.Role == "collaborator")
            .Select(u => mapper.Map<CollaboratorDto>(u))
            .ToList();

        return Ok(new { collaborators });
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("collaborator/remove")]
    public async Task<IActionResult> DeleteCollaborator([FromBody] List<CollaboratorDto> collaboratorsDto)
    {
        if (collaboratorsDto == null || !collaboratorsDto.Any())
        {
            return BadRequest("No collaborators provided.");
        }

        var allUsers = await userService.GetAll();

        var emailsToDelete = collaboratorsDto.Select(dto => dto.Email).ToList();

        var usersToDelete = allUsers
            .Where(u => emailsToDelete.Contains(u.Email))
            .ToList();

        if (!usersToDelete.Any())
        {
            return NotFound("No users found to delete.");
        }

        var status = await userService.Delete(usersToDelete);

        return status ? Ok("Users have been deleted!") : BadRequest("Deleting users has failed");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        var searchedUser = await userService.Get(userDto.Email, userDto.Password);

        if (searchedUser == null) return Unauthorized(new { message = "User account not found!" });

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, searchedUser.Guid.ToString()),
            new Claim(ClaimTypes.Role, searchedUser.Role)
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

        return Ok(new { message = "User has logged in successfully", role = searchedUser.Role, email = searchedUser.Email });
    }
}