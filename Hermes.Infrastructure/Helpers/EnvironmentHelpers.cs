namespace Hermes.Infrastructure.Helpers;
public static class EnvironmentHelper
{
    public static string? GetValidationToken()
    {
        return Environment.GetEnvironmentVariable("VALIDATION_TOKEN");
    }

    public static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("ENVIRONMENT") == "Development";
    }
}