using FluentValidation;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Validators.Animals;

public class UpdateAnimalDtoValidator : AbstractValidator<AnimalDto>
{
    public UpdateAnimalDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(x => x.Description)
            .MaximumLength(350);

        RuleFor(x => x.Status)
            .IsInEnum();
        
        RuleFor(x => x.Age)
            .InclusiveBetween(0, 50);
        
        RuleFor(x => x.AdmissionDate)
            .LessThanOrEqualTo(timeProvider.GetUtcNow());
    }
}