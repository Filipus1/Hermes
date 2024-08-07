using Hermes.Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/email")]
public class EmailController : Controller
{
    private readonly IEmailService emailService;

    public EmailController(IEmailService emailService)
    {
        this.emailService = emailService;
    }


    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EmailDto emaildto)
    {
        var status = await emailService.Send(emaildto.ReceiverEmail, emaildto.Body);

        return status ? Ok(new { message = "Email has been sent succesfully" }) : BadRequest(new { message = "Sending email has failed" });
    }
}