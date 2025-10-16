using FluentValidation;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Adoptions;

namespace ShelterManager.Services.Validators.Adoptions;

public class UpdateAdoptionDtoValidator : AbstractValidator<UpdateAdoptionDto>
{
    public UpdateAdoptionDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid adoption status.");
        
        RuleFor(x => x.Note)
            .MaximumLength(350).WithMessage("Note must not exceed 350 characters.");

        RuleFor(x => x.Event)
            .NotNull()
            .When(x => x.Status == AdoptionStatus.Approved)
            .WithMessage("Event must be provided when adoption is approved.");
        
        RuleFor(x => x.Event!.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(250).WithMessage("Title must not exceed 100 characters.")
            .When(x => x.Event is not null);

        RuleFor(x => x.Event!.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(100).WithMessage("Location must not exceed 150 characters.")
            .When(x => x.Event is not null);

        RuleFor(x => x.Event!.Description)
            .MaximumLength(250).WithMessage("Description must not exceed 250 characters.")
            .When(x => x.Event is not null);

        RuleFor(x => x.Event!.PlannedAdoptionDate)
            .GreaterThanOrEqualTo(timeProvider.GetUtcNow())
            .WithMessage("Event date must be in the future")
            .When(x => x.Event is not null);
    }
}