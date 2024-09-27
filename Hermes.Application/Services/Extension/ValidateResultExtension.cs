using FluentValidation.Results;

namespace Hermes.Application.Services.Extension;
public static class ValidationResultExtensions
{
    public static IDictionary<string, string[]> ToDictionary(this ValidationResult result)
    {
        return result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }
}