namespace Hermes.Infrastructure;

public class TokenGenerator : ITokenGenerator
{
    public string GenerateToken()
    {
        return Guid.NewGuid().ToString();
    }
}

public interface ITokenGenerator
{
    public string GenerateToken();
}