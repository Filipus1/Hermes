namespace Hermes.Application.Entities.Dto;

public record EmailDto
{
    public string ReceiverEmail { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}