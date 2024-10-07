using AutoMapper;
using FluentValidation;
using Hermes.Application.Abstraction;
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
    private readonly IValidator<RegisterDto> _userValidator;
    private readonly IValidator<List<CollaboratorDto>> _collaboratorValidator;

    public UserController(IUserService userService
    , ITokenService tokenService, IValidator<RegisterDto> userValidator, IValidator<List<CollaboratorDto>> collaboratorValidator)
    {
        _userService = userService;
        _tokenService = tokenService;
        _userValidator = userValidator;
        _collaboratorValidator = collaboratorValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var isTokenValid = await _tokenService.Validate(dto.Token);

        if (!isTokenValid)
        {
            return BadRequest("Invalid token");
        }

        var userValidationResult = await _userValidator.ValidateAsync(dto);

        if (!userValidationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(userValidationResult.ToDictionary()));
        }

        await _userService.Create(dto);
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
        var collaboratorValidationResult = await _collaboratorValidator.ValidateAsync(dto);

        if (!collaboratorValidationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(collaboratorValidationResult.ToDictionary()));
        }

        await _userService.Delete(dto);

        return Ok(new { message = "Users have been deleted" });
    }
}