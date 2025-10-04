using FluentValidation;
using ShelterManager.Services.Dtos.Events;
using ShelterManager.Services.Validators.Commons;

namespace ShelterManager.Services.Validators.Events;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(timeProvider.GetUtcNow())
            .WithMessage("Event date must be in the future");
        
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

        RuleFor(x => x.AnimalId)
            .NotEmpty().WithMessage("Animal ID is required.");
        
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Cost)
            .SetValidator(new MoneyDtoValidator()!)
            .When(x => x.Cost != null);
    }
}