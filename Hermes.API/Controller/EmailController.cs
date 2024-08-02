using Hermes.Application.Abstraction;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/invite")]
public class EmailController : Controller
{
    private readonly IEmailService emailService;

    public EmailController(IEmailService eMailService)
    {
        this.emailService = eMailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EmailDto emaildto)
    {
        var status = await emailService.Send(emaildto.ReceiverEmail, emaildto.Body);

        return status ? Ok(new { message = "Email has been sent succesfully" }) : BadRequest(new { message = "Email has been sent succesfully" });
    }
}