using FluentValidation;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Validators.Animals;

public class CreateAnimalDtoValidator : AbstractValidator<CreateAnimalDto>
{
    public CreateAnimalDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Description)
            .MaximumLength(350);

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 50);

        RuleFor(x => x.AdmissionDate)
            .LessThanOrEqualTo(timeProvider.GetUtcNow());
    }
}