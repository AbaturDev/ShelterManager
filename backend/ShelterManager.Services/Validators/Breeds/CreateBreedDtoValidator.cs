using FluentValidation;
using ShelterManager.Services.Dtos.Breeds;

namespace ShelterManager.Services.Validators.Breeds;

public class CreateBreedDtoValidator : AbstractValidator<CreateBreedDto>
{
    public CreateBreedDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);
    }   
}