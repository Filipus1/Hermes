namespace Hermes.Infrastructure.Dto;
public record EmailDto
{
    public string ReceiverEmail { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}