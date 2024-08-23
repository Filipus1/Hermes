using AutoMapper;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, ITokenService tokenService, IMapper mapper)
    {
        _userService = userService;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var isTokenValid = await _tokenService.Validate(dto.Token);

        if (!isTokenValid)
        {
            return BadRequest("Invalid token");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User user = _mapper.Map<User>(dto);

        if (await _userService.Get(user.Email) != null)
        {
            return Conflict(new { message = "An account with this email already exists" });
        }

        var status = await _userService.Create(user);

        if (!status)
        {
            return BadRequest(new { message = "Failed to create user" });
        }

        await _tokenService.Use(dto.Token);

        return Ok(new { message = "User has been created successfully" });
    }

    [Authorize(Roles = "admin")]
    [HttpGet("collaborators")]
    public async Task<IActionResult> GetCollaborators()
    {
        var users = await _userService.GetAll();
        var collaborators = users
            .Where(u => u.Role == "collaborator")
            .Select(u => _mapper.Map<CollaboratorDto>(u))
            .ToList();

        return Ok(new { collaborators });
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("collaborator/remove")]
    public async Task<IActionResult> DeleteCollaborator([FromBody] List<CollaboratorDto> dto)
    {
        if (dto == null || dto.Count == 0)
        {
            return BadRequest("No collaborators provided.");
        }

        var allUsers = await _userService.GetAll();

        var emails = dto.Select(dto => dto.Email).ToList();

        var usersToDelete = allUsers
            .Where(u => emails.Contains(u.Email))
            .ToList();

        if (usersToDelete.Count == 0)
        {
            return NotFound("No users found to delete.");
        }

        var status = await _userService.Delete(usersToDelete);

        return status ? Ok(new { message = "Users have been deleted" }) : BadRequest(new { message = "Deleting users have failed" });
    }
}