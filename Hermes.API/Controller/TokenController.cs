using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API;
[ApiController]
[Route("api/invite")]
public class TokenController : Controller
{
    public readonly ITokenService tokenService;

    public TokenController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    [Route("generate")]
    public async Task<IActionResult> GenerateInviteToken([FromBody] InviteDto dto)
    {
        try
        {
            var invitationToken = await tokenService.Create(dto.CreatedBy);

            return Ok(new { token = invitationToken!.Token, createdBy = invitationToken!.CreatedBy });
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            return BadRequest(new { message = $"Generating invite token has failed: ${e.Message}" });
        }
    }

    [HttpPost]
    [Route("validate")]
    public async Task<IActionResult> ValidateInviteToken([FromBody] TokenDto dto)
    {
        try
        {
            var validation = await tokenService.Validate(dto.Token);

            if (validation)
            {
                var token = await tokenService.Get(dto.Token);

                return Ok(new { createdBy = token!.CreatedBy });
            }

            return BadRequest(new { message = $"Token {dto.Token} is not valid" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Generating invite token has failed: ${e.Message}" });
        }
    }

    [HttpPost]
    [Route("use")]
    public async Task<IActionResult> UseInviteToken([FromBody] TokenDto dto)
    {
        try
        {
            await tokenService.Use(dto.Token);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Using invite token has failed: ${e.Message}" });
        }
    }
}
