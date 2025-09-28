using FluentValidation;
using ShelterManager.Services.Dtos.Accounts;

namespace ShelterManager.Services.Validators.Accounts;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(x => x.Surname)
            .NotEmpty()
            .MaximumLength(30);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}