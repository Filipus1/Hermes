using FluentValidation;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Services.Validation;
public class AuthValidator : AbstractValidator<UserDto>
{
    public AuthValidator(IUserRepository repository, IUserService userService)
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format")
            .MustAsync(async (email, _) => await repository.IsUserRegistered(email))
            .WithMessage("An account with this email does not exist");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(30).WithMessage("Password must be at most 30 characters long")
            .Matches(@"(?=.*\d)(?=.*[A-Z])").WithMessage("Password must contain at least one number and one uppercase letter");

        RuleFor(u => u).MustAsync(async (dto, _) =>
        {
            var user = await userService.GetUser(dto);
            return user != null;
        })
            .WithMessage("Email or password is incorrect")
            .WithName("auth");
    }
}
