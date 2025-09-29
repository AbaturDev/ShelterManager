using FluentValidation;
using ShelterManager.Services.Dtos.Accounts;

namespace ShelterManager.Services.Validators.Accounts;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEqual(x => x.CurrentPassword);
    }
}