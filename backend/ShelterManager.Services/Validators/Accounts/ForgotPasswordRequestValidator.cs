using FluentValidation;
using ShelterManager.Services.Dtos.Accounts;

namespace ShelterManager.Services.Validators.Accounts;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address.");
    }
}