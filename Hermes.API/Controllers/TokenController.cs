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
        try
        {
            var invitationToken = await _tokenService.Create(dto.CreatedBy);

            return Ok(new { token = invitationToken!.Token, createdBy = invitationToken!.CreatedBy });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Generating invite token has failed: ${e.Message}" });
        }
    }

    [HttpPost]
    [Route("validate")]
    public async Task<IActionResult> ValidateInviteToken([FromBody] TokenDto dto)
    {
        try
        {
            var validation = await _tokenService.Validate(dto.Token);

            if (validation)
            {
                var token = await _tokenService.Get(dto.Token);

                return Ok(new { createdBy = token!.CreatedBy });
            }

            return BadRequest(new { message = $"Token {dto.Token} is not valid" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Generating invite token has failed: ${e.Message}" });
        }
    }
}
