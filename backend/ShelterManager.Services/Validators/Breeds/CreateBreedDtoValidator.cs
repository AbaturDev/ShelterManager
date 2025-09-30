using FluentValidation;
using ShelterManager.Services.Dtos.Breeds;

namespace ShelterManager.Services.Validators.Breeds;

public class CreateBreedDtoValidator : AbstractValidator<CreateBreedDto>
{
    public CreateBreedDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name must not exceed 30 characters");
    }   
}