using FluentValidation;
using ShelterManager.Services.Dtos.DailyTask;

namespace ShelterManager.Services.Validators.DailyTask;

public class AddDefaultDailyTaskEntryDtoValidator : AbstractValidator<AddDefaultDailyTaskEntryDto>
{
    public AddDefaultDailyTaskEntryDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long")
            .MaximumLength(30).WithMessage("Title must not exceed 30 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description must not exceed 300 characters");
    }
}