using FluentValidation;
using Hermes.Application.Services.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.Application.Services.Extension;
public static class IServiceCollectionExtension
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(UserValidationService).Assembly);

        return services;
    }
}