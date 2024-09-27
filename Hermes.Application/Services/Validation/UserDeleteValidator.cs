using FluentValidation;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Services.Validation;

public class UserDeleteValidator : AbstractValidator<List<CollaboratorDto>>
{
    public UserDeleteValidator()
    {
        RuleFor(collabList => collabList).NotEmpty().WithMessage("No users were provided");

        RuleForEach(collabList => collabList)
            .Must(collab => collab.Role != "admin")
            .WithMessage("Cannot delete admin users")
            .WithName("deleteCollaborator");
    }
}