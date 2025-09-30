using FluentValidation;
using ShelterManager.Services.Dtos.Species;

namespace ShelterManager.Services.Validators.Species;

public class CreateSpeciesDtoValidator : AbstractValidator<CreateSpeciesDto>
{
    public CreateSpeciesDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name must not exceed 30 characters");
    }
}