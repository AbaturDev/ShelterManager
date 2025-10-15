using FluentValidation;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Validators.Animals;

public class CreateAnimalDtoValidator : AbstractValidator<CreateAnimalDto>
{
    public CreateAnimalDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name must not exceed 30 characters");

        RuleFor(x => x.Description)
            .MaximumLength(350).WithMessage("Description must not exceed 350 characters");

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 50).WithMessage("Age must be between 0 and 50");

        RuleFor(x => x.Sex)
            .IsInEnum().WithMessage("Sex must be valid enum value");
        
        RuleFor(x => x.AdmissionDate)
            .LessThanOrEqualTo(timeProvider.GetUtcNow())
            .WithMessage("Admission date cannot be in the future");
    }
}