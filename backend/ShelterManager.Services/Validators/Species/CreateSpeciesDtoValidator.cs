using FluentValidation;
using ShelterManager.Services.Dtos.Species;

namespace ShelterManager.Services.Validators.Species;

public class CreateSpeciesDtoValidator : AbstractValidator<CreateSpeciesDto>
{
    public CreateSpeciesDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);
    }
}