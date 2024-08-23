using Hermes.Application.Abstraction;
using Hermes.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API;
[ApiController]
[Route("api/email")]
public class EmailController : Controller
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EmailDto emaildto)
    {
        var status = await _emailService.Send(emaildto.ReceiverEmail, emaildto.Body);

        return status ? Ok(new { message = "Email has been sent succesfully" }) : BadRequest(new { message = "Sending email has failed" });
    }
}