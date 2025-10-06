using FluentValidation;
using ShelterManager.Database.Constants;
using ShelterManager.Services.Dtos.Accounts;

namespace ShelterManager.Services.Validators.Accounts;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name must not exceed 30 characters");
        
        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required")
            .MaximumLength(30).WithMessage("Surname must not exceed 30 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Role)
            .Custom((role, context) =>
            {
                if (!RoleNames.AllRoles.Contains(role))
                {
                    context.AddFailure("Invalid role", $"Role does not exist. Role must be in {string.Join(",", RoleNames.AllRoles)}");
                }
            });
    }
}