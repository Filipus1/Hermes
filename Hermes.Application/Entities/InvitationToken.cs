namespace Hermes.Application;

public class InvitationToken
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
    public DateTime ExpiryDate { get; set; }
}
