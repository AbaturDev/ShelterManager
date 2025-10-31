using FluentValidation;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Validators.Animals;

public class UpdateAnimalDtoValidator : AbstractValidator<UpdateAnimalDto>
{
    public UpdateAnimalDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name must not exceed 30 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(350)
            .WithMessage("Description must not exceed 350 characters");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid value from AnimalStatus enum");
        
        RuleFor(x => x.Age)
            .InclusiveBetween(0, 100)
            .WithMessage("Age must be between 0 and 100");
        
        RuleFor(x => x.AdmissionDate)
            .LessThanOrEqualTo(timeProvider.GetUtcNow())
            .WithMessage("Admission date cannot be in the future");
    }
}