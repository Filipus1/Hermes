using FluentValidation;
using Hermes.Application.Entities.Dto;

namespace Hermes.Application.Services.Validation;
public class UserDeleteValidationService : AbstractValidator<List<CollaboratorDto>>
{
    public UserDeleteValidationService()
    {
        RuleFor(collabList => collabList).NotEmpty().WithMessage("No users were provided");

        RuleForEach(collabList => collabList)
            .Must(collab => collab.Role != "admin")
            .WithMessage("Cannot delete admin users")
            .WithName("deleteCollaborator");
    }
}