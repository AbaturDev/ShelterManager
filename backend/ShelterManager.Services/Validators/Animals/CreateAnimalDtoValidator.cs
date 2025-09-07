using FluentValidation;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Validators.Animals;

public class CreateAnimalDtoValidator : AbstractValidator<CreateAnimalDto>
{
    public CreateAnimalDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);
    }
}