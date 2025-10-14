using FluentValidation;
using ShelterManager.Services.Dtos.Adoptions;

namespace ShelterManager.Services.Validators.Adoptions;

public class CreateAdoptionDtoValidator : AbstractValidator<CreateAdoptionDto>
{
    public CreateAdoptionDtoValidator()
    {
        RuleFor(x => x.Note)
            .MaximumLength(350).WithMessage("Note must not exceed 350 characters.");
        
        RuleFor(x => x.Person.Name)
            .NotEmpty().WithMessage("Person name is required.")
            .MaximumLength(30).WithMessage("Person name must not exceed 30 characters.");
        
        RuleFor(x => x.Person.Surname)
            .NotEmpty().WithMessage("Person surname is required.")
            .MaximumLength(30).WithMessage("Person surname must not exceed 30 characters.");
        
        RuleFor(x => x.Person.PhoneNumber)
            .NotEmpty().WithMessage("Person phone number is required.")
            .MaximumLength(15).WithMessage("Phone number must not exceed 15 characters.");
        
        RuleFor(x => x.Person.Email)
            .NotEmpty().WithMessage("Person email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(50).WithMessage("Email must not exceed 50 characters.");
        
        RuleFor(x => x.Person.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street must not exceed 100 characters.");
        
        RuleFor(x => x.Person.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");
        
        RuleFor(x => x.Person.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters.");
    }
}