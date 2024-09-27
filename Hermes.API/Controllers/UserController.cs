using AutoMapper;
using FluentValidation;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Application.Entities.Dto;
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
    private readonly IValidator<User> _validator;

    public UserController(IUserService userService, ITokenService tokenService, IMapper mapper, IValidator<User> validator)
    {
        _userService = userService;
        _tokenService = tokenService;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var isTokenValid = await _tokenService.Validate(dto.Token);

        if (!isTokenValid)
        {
            return BadRequest("Invalid token");
        }

        User user = _mapper.Map<User>(dto);

        var userValidationResult = await _validator.ValidateAsync(user);

        if (!userValidationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(userValidationResult.ToDictionary()));
        }

        await _userService.Create(user);
        await _tokenService.MarkTokenAsUsed(dto.Token);

        return Ok(new { message = "User has been created successfully" });
    }

    [Authorize(Roles = "admin")]
    [HttpGet("collaborators")]
    public async Task<IActionResult> GetCollaborators()
    {
        var collaborators = await _userService.GetCollaborators();

        return Ok(new { collaborators });
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("collaborators")]
    public async Task<IActionResult> DeleteUser([FromBody] List<CollaboratorDto> dto)
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

        await _userService.Delete(usersToDelete);

        return Ok(new { message = "Users have been deleted" });
    }
}