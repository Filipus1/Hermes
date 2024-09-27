using FluentValidation;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Services.Validation;
public class AuthValidationService : AbstractValidator<UserDto>
{
    public AuthValidationService(IUserRepository repository)
    {
        RuleFor(u => u.Email).MustAsync(async (email, _) =>
        {
            return await repository.IsUserRegistered(email);
        })
            .WithMessage("An account with this email does not exist")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithMessage("Invalid email format");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(30).WithMessage("Password must be at most 30 characters long")
            .Matches(@"(?=.*\d)(?=.*[A-Z])").WithMessage("Password must contain at least one number and one uppercase letter");
    }
}