using FluentValidation;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Services;
public class UserValidationService : AbstractValidator<RegisterDto>
{
    public UserValidationService(IUserRepository repository)
    {
        RuleFor(u => u.Email).MustAsync(async (email, _) =>
        {
            return await repository.IsEmailUnique(email);

        }).WithMessage("An account with this email already exists");

        RuleFor(u => u.Password)
                 .NotEmpty().WithMessage("Password is required")
                 .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                 .MaximumLength(30).WithMessage("Password must be at most 30 characters long")
                 .Matches(@"(?=.*\d)(?=.*[A-Z])").WithMessage("Password must contain at least one number and one uppercase letter");
    }
}