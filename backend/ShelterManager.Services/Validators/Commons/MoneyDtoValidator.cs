using FluentValidation;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Validators.Commons;

public class MoneyDtoValidator : AbstractValidator<MoneyDto>
{
    public MoneyDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter code (ISO 4217).");
    }
}