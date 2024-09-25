using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;
[ApiController]
[Route("api/invite")]
public class TokenController : Controller
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [Route("generate")]
    public async Task<IActionResult> GenerateInviteToken([FromBody] InviteDto dto)
    {
        var invitationToken = await _tokenService.Create(dto.CreatedBy);

        return Ok(new { token = invitationToken!.Token });
    }

    [HttpGet]
    [Route("validate")]
    public async Task<IActionResult> ValidateInviteToken([FromQuery] string token)
    {
        var validation = await _tokenService.Validate(token);

        if (validation)
        {
            var searchedToken = await _tokenService.Get(token);

            return Ok(new { createdBy = searchedToken?.CreatedBy });
        }

        return BadRequest(new { message = $"Token {token} is not valid" });
    }
}
